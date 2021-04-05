using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    internal sealed class DbCollection<TDocument> : 
        IChangeTracker,
        IDbCollection<TDocument>,
        IDocumentTracker<TDocument>
        where TDocument : IMongoAggregate<TDocument>
    {
        private readonly ConcurrentDictionary<BsonDocument, TrackedDocument<TDocument>> _trackedDocuments = new();

        private readonly IClientSessionHandle _session;

        private readonly List<IDbCollectionListener<TDocument>> _listeners;
        private readonly PrimaryKeyFilterSelector<TDocument> _primaryKeyFilterSelector;

        public DbCollection(
            MongoContext context,
            string name,
            IMongoCollection<TDocument> collection,
            IClientSessionHandle session,
            List<IDbCollectionListener<TDocument>> listeners,
            PrimaryKeyFilterSelector<TDocument> primaryKeyFilterSelector)
        {
            Context = context;
            Name = name;
            Collection = collection;
            _session = session;
            _listeners = listeners;
            _primaryKeyFilterSelector = primaryKeyFilterSelector;
        }

        public string Name { get; }
        public IMongoCollection<TDocument> Collection { get; }

        public MongoContext Context { get; }

        public async Task<AsyncDelegate> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var context = new SaveDocumentsChangesContext<TDocument>();

            foreach (var trackedDocument in _trackedDocuments.Values)
            {
                var documentContext = new SaveDocumentChangesContext<TDocument>(trackedDocument, _primaryKeyFilterSelector);
                documentContext.OnSaveChanges(context);
            }

            var options = new BulkWriteOptions { IsOrdered = true };
            await Collection.BulkWriteAsync(_session, context.WriteModels, options, cancellationToken);

            return ct => OnChangesSavedAsync(context, ct);
        }

        private async Task OnChangesSavedAsync(
            SaveDocumentsChangesContext<TDocument> context,
            CancellationToken cancellationToken)
        {
            UpdateTrackedDocumentStates();

            var pendingEventsSavedContext = new PendingEventsSavedContext<TDocument>(context.PendingEvents);

            foreach (var listener in _listeners)
            {
                await listener.OnEventsSavedAsync(pendingEventsSavedContext, cancellationToken);
            }
        }

        private void UpdateTrackedDocumentStates()
        {
            foreach (var entry in _trackedDocuments)
            {
                UpdateTrackedDocumentState(entry.Value.State, entry);
            }
        }

        private void UpdateTrackedDocumentState(
            DocumentState state,
            KeyValuePair<BsonDocument, TrackedDocument<TDocument>> entry)
        {
            switch (state)
            {
                case DocumentState.Deleted:
                    break;
                case DocumentState.Added:
                case DocumentState.Modified:
                case DocumentState.Unchanged:
                    entry.Value.State = DocumentState.Unchanged;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public IFindFluent<TDocument> Find(FilterDefinition<TDocument> filter)
        {
            return new TrackingFindFluent<TDocument>(Collection.Find(filter), this);
        }

        public TDocument Attach(TDocument document)
        {
            return Attach(document, DocumentState.Unchanged).Document;
        }

        public void Add(TDocument document)
        {
            Attach(document, DocumentState.Added);
        }

        public void Remove(TDocument document)
        {
            Attach(document, DocumentState.Deleted);
        }

        private TrackedDocument<TDocument> Attach(TDocument document, DocumentState state)
        {
            var primaryKey = PrimaryKey(document);
            var trackedDocument = _trackedDocuments.GetOrAdd(primaryKey, CreateTrackedDocument, (document, state));
            trackedDocument.State = state;
            return trackedDocument;
        }

        private BsonDocument PrimaryKey(TDocument document)
        {
            var filter = GetPrimaryKeyFilter(document);

            var serializer = BsonSerializer.LookupSerializer<TDocument>();
            var serializerRegistry = BsonSerializer.SerializerRegistry;

            var bson = filter.Render(serializer, serializerRegistry);
            return bson;
        }

        private FilterDefinition<TDocument> GetPrimaryKeyFilter(TDocument document)
        {
            return _primaryKeyFilterSelector(Builders<TDocument>.Filter, document);
        }

        private static TrackedDocument<TDocument> CreateTrackedDocument(
            BsonDocument key,
            (TDocument, DocumentState) args)
        {
            var (document, state) = args;
            return new TrackedDocument<TDocument>(document, state);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    internal sealed class DbCollection<TDocument> : 
        IDbCollection,
        IDbCollection<TDocument>,
        IDocumentTracker<TDocument>
        where TDocument : IMongoAggregate<TDocument>
    {
        private readonly List<TrackedDocument<TDocument>> _trackedDocuments = new();
        private readonly Dictionary<BsonDocument, TrackedDocument<TDocument>> _trackedDocumentsById = new();

        private readonly IClientSessionHandle _session;

        private readonly List<IDbCollectionListener<TDocument>> _listeners;
        private readonly PrimaryKeyFilterSelector<TDocument> _primaryKeyFilterSelector;
        private readonly IReadOnlyList<IndexDefinition<TDocument>> _indexDefinitions;

        public DbCollection(
            MongoContext context,
            IMongoCollection<TDocument> collection,
            IClientSessionHandle session,
            List<IDbCollectionListener<TDocument>> listeners,
            DbCollectionDefinition<TDocument> definition)
        {
            Context = context;
            Name = definition.Name;
            Collection = collection;
            _session = session;
            _listeners = listeners;
            _primaryKeyFilterSelector = definition.PrimaryKeyFilterSelector;
            _indexDefinitions = definition.IndexDefinitions;
        }

        public string Name { get; }
        public IMongoCollection<TDocument> Collection { get; }

        public MongoContext Context { get; }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            var processor = new DbCollectionDefinitionProcessor<TDocument>(Collection);
            await processor.InitializeIndices(_indexDefinitions, cancellationToken);
        }

        public async Task<AsyncDelegate> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var context = new SaveDocumentsChangesContext<TDocument>();

            foreach (var trackedDocument in _trackedDocuments)
            {
                var documentContext = new SaveDocumentChangesContext<TDocument>(trackedDocument, _primaryKeyFilterSelector);
                documentContext.OnSaveChanges(context);
            }

            if (context.WriteModels.Count > 0)
            {
                var options = new BulkWriteOptions { IsOrdered = true };
                await Collection.BulkWriteAsync(_session, context.WriteModels, options, cancellationToken);                
            }

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
            foreach (var trackedDocument in _trackedDocuments)
            {
                UpdateTrackedDocumentState(trackedDocument.State, trackedDocument);
            }
        }

        private void UpdateTrackedDocumentState(
            DocumentState state,
            TrackedDocument<TDocument> trackedDocument)
        {
            switch (state)
            {
                case DocumentState.Deleted:
                    break;
                case DocumentState.Added:
                case DocumentState.Modified:
                case DocumentState.Unchanged:
                    trackedDocument.State = DocumentState.Unchanged;
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
            
            if (_trackedDocumentsById.TryGetValue(primaryKey, out var currentTrackedDocument))
            {
                currentTrackedDocument.State = state;
                return currentTrackedDocument;
            }
            
            var newTrackedDocument = new TrackedDocument<TDocument>(document, state);
            
            _trackedDocuments.Add(newTrackedDocument);
            _trackedDocumentsById.Add(primaryKey, newTrackedDocument);
            
            return newTrackedDocument;
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
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    internal sealed class MongoSet<TDocument> : 
        IMongoSet,
        IMongoSet<TDocument>,
        IDocumentTracker<TDocument>
        where TDocument : IMongoAggregate<TDocument>
    {
        private readonly List<TrackedDocument<TDocument>> _trackedDocuments = new();
        private readonly Dictionary<BsonDocument, TrackedDocument<TDocument>> _trackedDocumentsById = new();

        private readonly IClientSessionHandle _session;

        private readonly List<IMongoSetListener<TDocument>> _listeners;
        private readonly PrimaryKeyFilterSelector<TDocument> _primaryKeyFilterSelector;
        private readonly IReadOnlyList<IndexDefinition<TDocument>> _indexDefinitions;

        public MongoSet(
            MongoContext context,
            IMongoCollection<TDocument> collection,
            IClientSessionHandle session,
            List<IMongoSetListener<TDocument>> listeners,
            MongoSetDefinition<TDocument> definition)
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
            var processor = new MongoSetDefinitionProcessor<TDocument>(Collection);
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
            UpdateTrackedDocuments();

            var changesSavedContext = new ChangesSavedContext<TDocument>(context.Mutations);

            foreach (var listener in _listeners)
            {
                await listener.OnEventsSavedAsync(changesSavedContext, cancellationToken);
            }
        }

        private void UpdateTrackedDocuments()
        {
            var deletions = new List<(int, BsonDocument)>();
            
            for (var index = 0; index < _trackedDocuments.Count; index++)
            {
                var trackedDocument = _trackedDocuments[index];
                
                var document = trackedDocument.Document;
                var currentState = trackedDocument.State;
                
                var updatedState = NextDocumentState(document, currentState);
                trackedDocument.State = updatedState;
                
                if (updatedState == DocumentState.Deleted)
                {
                    deletions.Add((index, PrimaryKey(document)));
                }
            }

            foreach (var (index, key) in deletions)
            {
                _trackedDocuments.RemoveAt(index);
                _trackedDocumentsById.Remove(key);
            }
        }

        private DocumentState NextDocumentState(TDocument document, DocumentState currentState)
        {
            switch (currentState)
            {
                case DocumentState.Deleted:
                    return currentState;
                case DocumentState.Added:
                case DocumentState.Modified:
                case DocumentState.Unchanged:
                    return DocumentState.Unchanged;
                default:
                    throw new InvalidOperationException($"The tracked document '{PrimaryKey(document)}' specified an unexpected state '{currentState}'");
            }
        }

        public IFindFluent<TDocument> Find(FilterDefinition<TDocument> filter)
        {
            return new TrackingFindFluent<TDocument>(Collection.Find(filter), this);
        }

        public IFindFluent<TDocument> ToFindFluent()
        {
            return Find(FilterDefinition<TDocument>.Empty);
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
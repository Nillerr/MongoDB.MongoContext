using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public abstract class MongoContext
    {
        private readonly ConcurrentDictionary<string, IDbCollection> _collectionContexts = new();
        
        private readonly IMongoDatabase _database;
        private readonly IClientSessionHandle _session;
        private readonly IReadOnlyList<IDbCollectionListenerFactory> _collectionListenerFactories;

        protected MongoContext(DatabaseContextOptions options)
        {
            _database = options.Database;
            _session = options.Database.Client.StartSession();
            _collectionListenerFactories = options.CollectionListenerFactories;
        }

        internal IMongoDatabase Database => _database;

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            foreach (var collectionContext in _collectionContexts.Values)
            {
                await collectionContext.InitializeAsync(cancellationToken);
            }
        }

        protected virtual DbCollectionDefinition<TDocument> Collection<TDocument>(
            string name,
            PrimaryKeyFilterSelector<TDocument> primaryKeyFilterSelector)
            where TDocument : IMongoAggregate<TDocument>
        {
            return new DbCollectionDefinition<TDocument>(this, name, primaryKeyFilterSelector);
        }

        internal virtual IDbCollection<TDocument> GetCollection<TDocument>(DbCollectionDefinition<TDocument> definition)
            where TDocument : IMongoAggregate<TDocument>
        {
            IDbCollection collection = _collectionContexts.GetOrAdd(definition.Name, CreateCollection, definition);
            return (IDbCollection<TDocument>) collection;
        }

        private DbCollection<TDocument> CreateCollection<TDocument>(
            string name,
            DbCollectionDefinition<TDocument> definition)
            where TDocument : IMongoAggregate<TDocument>
        {
            var collection = _database.GetCollection<TDocument>(name);
            
            var listeners = _collectionListenerFactories
                .Select(factory => factory.CreateListener<TDocument>(name))
                .ToList();
            
            var collectionContext = new DbCollection<TDocument>(this, collection, _session, listeners, definition);
            return collectionContext;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var postCommitActions = new List<AsyncDelegate>();

            _session.StartTransaction();

            try
            {
                foreach (var collection in _collectionContexts.Values)
                {
                    var postCommitAction = await collection.SaveChangesAsync(cancellationToken);
                    postCommitActions.Add(postCommitAction);
                }

                await _session.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception)
            {
                // ReSharper disable once MethodSupportsCancellation
                await _session.AbortTransactionAsync();
                
                throw;
            }

            foreach (var postCommitAction in postCommitActions)
            {
                await postCommitAction(cancellationToken);
            }
        }
    }
}
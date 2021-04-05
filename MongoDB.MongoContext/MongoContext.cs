using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public abstract class MongoContext
    {
        private readonly ConcurrentDictionary<string, IMongoSet> _collectionContexts = new();
        
        private readonly IMongoDatabase _database;
        private readonly IClientSessionHandle _session;

        protected MongoContext(DatabaseContextOptions options)
        {
            _database = options.Database;
            _session = options.Database.Client.StartSession();
        }

        public IMongoDatabase Database => _database;

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            foreach (var collectionContext in _collectionContexts.Values)
            {
                await collectionContext.InitializeAsync(cancellationToken);
            }
        }

        protected MongoSetDefinition<TDocument> Collection<TDocument>(
            string name,
            PrimaryKeyFilterSelector<TDocument> primaryKeyFilterSelector)
            where TDocument : IMongoAggregate<TDocument>
        {
            return new MongoSetDefinition<TDocument>(this, name, primaryKeyFilterSelector);
        }

        internal IMongoSet<TDocument> GetCollection<TDocument>(MongoSetDefinition<TDocument> definition)
            where TDocument : IMongoAggregate<TDocument>
        {
            IMongoSet collection = _collectionContexts.GetOrAdd(definition.Name, CreateCollection, definition);
            return (IMongoSet<TDocument>) collection;
        }

        private MongoSet<TDocument> CreateCollection<TDocument>(
            string name,
            MongoSetDefinition<TDocument> definition)
            where TDocument : IMongoAggregate<TDocument>
        {
            var collection = _database.GetCollection<TDocument>(name);
            
            var collectionContext = new MongoSet<TDocument>(this, collection, _session, definition);
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
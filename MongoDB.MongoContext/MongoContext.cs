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
        private readonly ConcurrentDictionary<string, IChangeTracker> _collectionContexts = new();

        private readonly IReadOnlyList<IDbCollectionListenerFactory> _collectionListenerFactories;

        protected MongoContext(DatabaseContextOptions options)
        {
            Database = options.Database;
            Session = options.Database.Client.StartSession();

            _collectionListenerFactories = options.CollectionListenerFactories;
        }

        protected IMongoDatabase Database { get; }
        protected IClientSessionHandle Session { get; }

        protected virtual IDbCollection<TDocument> GetCollection<TDocument>(
            string name,
            PrimaryKeyFilterSelector<TDocument> primaryKeyFilterSelector)
            where TDocument : IMongoAggregate<TDocument>
        {
            IChangeTracker collection = _collectionContexts.GetOrAdd(name, CreateCollection, primaryKeyFilterSelector);
            return (IDbCollection<TDocument>) collection;
        }

        private DbCollection<TDocument> CreateCollection<TDocument>(
            string name,
            PrimaryKeyFilterSelector<TDocument> primaryKeyFilterSelector)
            where TDocument : IMongoAggregate<TDocument>
        {
            var collection = Database.GetCollection<TDocument>(name);
            
            var listeners = _collectionListenerFactories
                .Select(factory => factory.CreateListener<TDocument>(name))
                .ToList();
            
            var collectionContext = new DbCollection<TDocument>(this, name, collection, Session, listeners, primaryKeyFilterSelector);
            return collectionContext;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var postCommitActions = new List<AsyncDelegate>();

            Session.StartTransaction();

            try
            {
                foreach (var collection in _collectionContexts.Values)
                {
                    var postCommitAction = await collection.SaveChangesAsync(cancellationToken);
                    postCommitActions.Add(postCommitAction);
                }

                await Session.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception)
            {
                // ReSharper disable once MethodSupportsCancellation
                await Session.AbortTransactionAsync();
                
                throw;
            }

            foreach (var postCommitAction in postCommitActions)
            {
                await postCommitAction(cancellationToken);
            }
        }
    }
}
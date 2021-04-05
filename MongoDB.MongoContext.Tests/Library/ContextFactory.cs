using System.Collections.Concurrent;
using System.Threading.Tasks;
using MongoDB.Driver;
using Xunit;

namespace MongoDB.MongoContext.Tests
{
    public abstract class ContextFactory<TContext> : IAsyncLifetime, IContextManager
        where TContext : MongoContext
    {
        private readonly ConcurrentDictionary<string, ContextFixture<TContext>> _contextFixtures = new();
        
        protected abstract MongoClient CreateClient();

        protected abstract TContext CreateContext(DatabaseContextOptions options);

        public ContextFixture<TContext> CreateContextFixture(string name, params IDbCollectionListenerFactory[] collectionListenerFactories)
        {
            var client = CreateClient();
            var database = client.GetDatabase(name);
            var options = new DatabaseContextOptions(database, collectionListenerFactories);
            var contextFixture = _contextFixtures.GetOrAdd(name, CreateContextFixture, options);
            return contextFixture;
        }

        private ContextFixture<TContext> CreateContextFixture(string name, DatabaseContextOptions options)
        {
            var context = CreateContext(options);
            return new ContextFixture<TContext>(this, name, context);
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            var client = CreateClient();
            
            foreach (var database in _contextFixtures.Keys)
            {
                await client.DropDatabaseAsync(database);
            }

            _contextFixtures.Clear();
        }

        public async Task DestroyContext(string name)
        {
            var client = CreateClient();
            
            if (_contextFixtures.TryRemove(name, out _))
            {
                await client.DropDatabaseAsync(name);
            }
        }
    }
}
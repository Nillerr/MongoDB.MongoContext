using System.Collections.Concurrent;
using System.Threading.Tasks;
using MongoDB.Driver;
using Xunit;

namespace MongoDB.MongoContext.Tests
{
    public sealed class ArticlesContextFixture : IAsyncLifetime
    {
        private readonly ConcurrentDictionary<string, IMongoDatabase> _managedDatabases = new();
        
        private readonly IMongoClient _client;
        
        public ArticlesContextFixture()
        {
            _client = new MongoClient("mongodb://root:example@localhost:27017/?authSource=admin");
        }

        public ArticlesContext CreateContext<T>(T testClassInstance, params IDbCollectionListenerFactory[] collectionListenerFactories)
            where T : notnull
        {
            var testClassName = testClassInstance.GetType().Name;
            return CreateContext(testClassName, collectionListenerFactories);
        }

        private ArticlesContext CreateContext(string name, params IDbCollectionListenerFactory[] collectionListenerFactories)
        {
            var database = _managedDatabases.GetOrAdd(name, _client.GetDatabase, (MongoDatabaseSettings?) null);
            var options = new DatabaseContextOptions(database, collectionListenerFactories);
            var context = new ArticlesContext(options);
            return context;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            foreach (var database in _managedDatabases.Keys)
            {
                await _client.DropDatabaseAsync(database);
            }

            _managedDatabases.Clear();
        }
    }
}
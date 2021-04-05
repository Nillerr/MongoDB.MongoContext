using System.Collections.Concurrent;
using System.Threading.Tasks;
using MongoDB.Driver;
using Xunit;

namespace MongoDB.MongoContext.Tests
{
    public sealed class ArticlesContextFactoryFixture : IAsyncLifetime, IArticlesContextManager
    {
        private readonly ConcurrentDictionary<string, ArticlesContextFixture> _contextFixtures = new();
        
        private readonly IMongoClient _client;
        
        public ArticlesContextFactoryFixture()
        {
            _client = new MongoClient("mongodb://root:example@localhost:27017/?authSource=admin");
        }

        public ArticlesContextFixture CreateContext(string name, params IDbCollectionListenerFactory[] collectionListenerFactories)
        {
            var database = _client.GetDatabase(name);
            var options = new DatabaseContextOptions(database, collectionListenerFactories);
            var contextFixture = _contextFixtures.GetOrAdd(name, CreateContext, options);
            return contextFixture;
        }

        private ArticlesContextFixture CreateContext(string name, DatabaseContextOptions options)
        {
            var context = new ArticlesContext(options);
            return new ArticlesContextFixture(this, name, context);
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            foreach (var database in _contextFixtures.Keys)
            {
                await _client.DropDatabaseAsync(database);
            }

            _contextFixtures.Clear();
        }

        public async Task DestroyContext(string name)
        {
            if (_contextFixtures.TryRemove(name, out _))
            {
                await _client.DropDatabaseAsync(name);
            }
        }
    }
}
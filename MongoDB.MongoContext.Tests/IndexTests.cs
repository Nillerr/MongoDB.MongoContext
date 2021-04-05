using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace MongoDB.MongoContext.Tests
{
    public class IndexTests : Tests, IClassFixture<ArticlesContextFactory>
    {
        private readonly string _contextName;
        private readonly ArticlesContextFactory _contextFactory;
        private readonly ArticlesContext _context;

        public IndexTests(ArticlesContextFactory contextFactory)
        {
            _contextName = GetType().Name;
            _contextFactory = contextFactory;
            _context = contextFactory.CreateUnmanagedContext(_contextName);
        }

        private IMongoIndexManager<Article> IndexManager => _context.Articles.Collection.Indexes;

        protected override async Task OnDisposeAsync()
        {
            await _contextFactory.DestroyContext(_contextName);
        }

        [Fact]
        public async Task Initialize_ShouldCreateIndexes()
        {
            // Arrange
            await CreateTextIndexAsync();
            await CreateCreatedAtIndexAsync();

            var expectedIndexes = await ListIndexesAsync();
            
            await DropAllIndexesAsync();

            // Act
            await _context.InitializeAsync();
            
            // Assert
            var indexes = await ListIndexesAsync();
            Assert.Equal(expectedIndexes, indexes);
        }

        private async Task CreateTextIndexAsync()
        {
            var idx = Builders<Article>.IndexKeys;

            var indexKeys = idx.Combine(
                idx.Text(e => e.Title),
                idx.Text(e => e.Body)
            );

            var options = new CreateIndexOptions();
            options.Weights = new BsonDocument
            {
                ["Body"] = 1,
                ["Title"] = 5,
            };
            
            var indexModel = new CreateIndexModel<Article>(indexKeys, options);
            
            await IndexManager.CreateOneAsync(indexModel);
        }

        private async Task CreateCreatedAtIndexAsync()
        {
            var idx = Builders<Article>.IndexKeys;
            
            var indexKeys = idx.Ascending(e => e.CreatedAt);
            
            var createdAtIndex = new CreateIndexModel<Article>(indexKeys);
            
            await IndexManager.CreateOneAsync(createdAtIndex);
        }

        private async Task<List<BsonDocument>> ListIndexesAsync()
        {
            var cursor = await IndexManager.ListAsync();
            var indexes = await cursor.ToListAsync();
            return indexes;
        }

        private async Task DropAllIndexesAsync()
        {
            await IndexManager.DropAllAsync();
        }
    }
}
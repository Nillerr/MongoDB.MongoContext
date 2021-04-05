using System.Threading.Tasks;
using MongoDB.Driver;
using Xunit;

namespace MongoDB.MongoContext.Tests
{
    public static class MongoCollectionAssertions
    {
        public static async Task ShouldBeEquivalentToAsync<TDocument>(
            this IMongoCollection<TDocument> collection,
            TDocument[] expected)
        {
            var actual = await collection
                .Find(FilterDefinition<TDocument>.Empty)
                .ToListAsync();
            
            Assert.Equal(expected, actual);
        }
        
        public static async Task ShouldBeEmptyAsync<TDocument>(
            this IMongoCollection<TDocument> collection)
        {
            var actual = await collection
                .Find(FilterDefinition<TDocument>.Empty)
                .ToListAsync();
            
            Assert.Empty(actual);
        }
    }
}
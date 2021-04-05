using System.Threading.Tasks;
using MongoDB.Driver;
using Xunit;

namespace MongoDB.MongoContext.Tests
{
    public static class MongoCollectionAssertions
    {
        public static async Task ShouldOnlyContainAsync<TDocument>(
            this IMongoCollection<TDocument> collection,
            params TDocument[] expected)
        {
            var actual = await collection
                .Find(FilterDefinition<TDocument>.Empty)
                .ToListAsync();
            
            Assert.Equal(expected, actual.ToArray());
        }
    }
}
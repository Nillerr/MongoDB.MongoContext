using MongoDB.Driver;

namespace MongoDB.MongoContext.Tests
{
    public sealed class ArticlesContextFactory : ContextFactory<ArticlesContext>
    {
        protected override MongoClient CreateClient()
        {
            return new MongoClient("mongodb://root:example@localhost:27017/?authSource=admin");
        }

        protected override ArticlesContext CreateContext(DatabaseContextOptions options)
        {
            return new ArticlesContext(options);
        }
    }
}
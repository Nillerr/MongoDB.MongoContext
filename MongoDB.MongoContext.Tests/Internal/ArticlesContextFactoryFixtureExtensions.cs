namespace MongoDB.MongoContext.Tests
{
    public static class ArticlesContextFactoryFixtureExtensions
    {
        public static ArticlesContext CreateContext<T>(
            this ArticlesContextFactoryFixture contextFactory,
            T tests,
            params IDbCollectionListenerFactory[] collectionListenerFactories)
            where T : IAsyncLifetimeManager
        {
            var testClassName = tests.GetType().Name;
            var contextFixture = contextFactory.CreateContext(testClassName, collectionListenerFactories);
            tests.AddAsyncLifetime(contextFixture);
            return contextFixture.Context;
        }
    }
}
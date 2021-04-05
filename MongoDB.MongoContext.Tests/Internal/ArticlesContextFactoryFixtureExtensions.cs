namespace MongoDB.MongoContext.Tests
{
    public static class ArticlesContextFactoryFixtureExtensions
    {
        public static ArticlesContext CreateContext<T>(
            this ArticlesContextFactoryFixture contextFactory,
            T tests,
            params IDbCollectionListenerFactory[] collectionListenerFactories)
            where T : IAsyncDisposableManager
        {
            var testClassName = tests.GetType().Name;
            var contextFixture = contextFactory.CreateContext(testClassName, collectionListenerFactories);
            tests.AddAsyncDisposable(contextFixture);
            return contextFixture.Context;
        }
    }
}
namespace MongoDB.MongoContext.Tests
{
    public static class ContextFactoryFixtureExtensions
    {
        /// <summary>
        /// Creates an instance of the context, attaching its lifetime to the lifetime of the lifetime manager.
        /// </summary>
        /// <param name="contextFactory"></param>
        /// <param name="lifetimeManager"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public static TContext CreateContext<T, TContext>(
            this ContextFactory<TContext> contextFactory,
            T lifetimeManager)
            where T : IAsyncLifetimeManager
            where TContext : MongoContext
        {
            var testClassName = lifetimeManager.GetType().Name;
            var contextFixture = contextFactory.CreateContextFixture(testClassName);
            lifetimeManager.AddAsyncLifetime(contextFixture);
            return contextFixture.Context;
        }
    }
}
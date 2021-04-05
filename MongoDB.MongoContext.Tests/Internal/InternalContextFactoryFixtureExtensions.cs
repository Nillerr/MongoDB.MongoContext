namespace MongoDB.MongoContext.Tests
{
    public static class InternalContextFactoryFixtureExtensions
    {
        /// <summary>
        /// Creates an instance of the context without managing its lifetime.
        /// </summary>
        /// <param name="contextFactory"></param>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        public static TContext CreateUnmanagedContext<TContext>(
            this ContextFactory<TContext> contextFactory,
            string name)
            where TContext : MongoContext
        {
            var contextFixture = contextFactory.CreateContextFixture(name);
            return contextFixture.Context;
        }
    }
}
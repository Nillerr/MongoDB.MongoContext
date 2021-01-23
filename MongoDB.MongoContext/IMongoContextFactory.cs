namespace MongoDB.MongoContext
{
    /// <summary>
    /// Creates MongoDB context instances of type <see cref="TContext"/>, given the configured
    /// <see cref="MongoContextOptions"/>. 
    /// </summary>
    /// <typeparam name="TContext">The type of MongoDB context.</typeparam>
    public interface IMongoContextFactory<out TContext>
        where TContext : MongoContext
    {
        /// <summary>
        /// Creates a new instance of <see cref="TContext"/> given the configured <see cref="MongoContextOptions"/>.
        /// </summary>
        /// <param name="options">The MongoDB context options.</param>
        /// <returns>The created <see cref="TContext"/>.</returns>
        TContext CreateContext(MongoContextOptions options);
    }
}
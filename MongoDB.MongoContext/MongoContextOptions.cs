using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    /// <summary>
    /// The options passed to the constructor of <see cref="MongoContext"/>.
    /// </summary>
    public sealed class MongoContextOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoContextOptions"/> class.
        /// </summary>
        /// <param name="database">The database to connect to.</param>
        public MongoContextOptions(IMongoDatabase database)
        {
            Database = database;
        }

        /// <summary>
        /// The database to connect to.
        /// </summary>
        public IMongoDatabase Database { get; set; }
        
        /// <summary>
        /// The options to apply when creating sessions, or <see langword="null"/> to use the default options.
        /// </summary>
#if NETSTANDARD2_1
        public ClientSessionOptions? SessionOptions { get; set; }
#else
        public ClientSessionOptions SessionOptions { get; set; }
#endif
    }
}
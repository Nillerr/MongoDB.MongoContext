using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public sealed class DatabaseContextOptions
    {
        public DatabaseContextOptions(IMongoDatabase database)
        {
            Database = database;
        }

        public IMongoDatabase Database { get; }
    }
}
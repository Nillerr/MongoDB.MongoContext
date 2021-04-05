using System.Collections.Generic;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public sealed class DatabaseContextOptions
    {
        public DatabaseContextOptions(IMongoDatabase database, IReadOnlyList<IDbCollectionListenerFactory> collectionListenerFactories = null)
        {
            Database = database;
            CollectionListenerFactories = collectionListenerFactories;
        }

        public IMongoDatabase Database { get; }

        public IReadOnlyList<IDbCollectionListenerFactory> CollectionListenerFactories { get; }
    }
}
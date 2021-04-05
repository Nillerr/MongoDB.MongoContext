namespace MongoDB.MongoContext
{
    public interface IMongoSetListenerFactory
    {
        IMongoSetListener<TDocument> CreateListener<TDocument>(string collectionName);
    }
}
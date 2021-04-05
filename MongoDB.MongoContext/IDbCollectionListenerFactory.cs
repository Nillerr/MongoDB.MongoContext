namespace MongoDB.MongoContext
{
    public interface IDbCollectionListenerFactory
    {
        IDbCollectionListener<TDocument> CreateListener<TDocument>(string collectionName);
    }
}
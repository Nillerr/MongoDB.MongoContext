using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public interface IDbCollection<TDocument>
    {
        string Name { get; }
        
        IMongoCollection<TDocument> Collection { get; }

        MongoContext Context { get; }

        IFindFluent<TDocument> Find(FilterDefinition<TDocument> filter);

        void Add(TDocument document);

        void Remove(TDocument document);
    }
}
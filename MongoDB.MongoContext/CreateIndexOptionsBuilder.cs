using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public sealed class CreateIndexOptionsBuilder<TDocument>
    {
        private readonly IMongoCollection<TDocument> _collection;

        public CreateIndexOptionsBuilder(IMongoCollection<TDocument> collection)
        {
            _collection = collection;
        }

        public WeightsBuilder<TDocument> Weights()
        {
            return new WeightsBuilder<TDocument>(_collection);
        }
    }
}
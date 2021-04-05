using System;
using MongoDB.Bson;

namespace MongoDB.MongoContext
{
    public class WeightsBuilder<TDocument>
    {
        public static implicit operator BsonDocument(WeightsBuilder<TDocument> source)
        {
            return new BsonDocument();
        }
    }
}
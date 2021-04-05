using System;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public class WeightsBuilder<TDocument>
    {
        private readonly BsonDocument _document = new();
        
        private readonly IMongoCollection<TDocument> _collection;

        public WeightsBuilder(IMongoCollection<TDocument> collection)
        {
            _collection = collection;
        }

        public WeightsBuilder<TDocument> Assign(Expression<Func<TDocument, object?>> expression, int weight)
        {
            var fieldDefinition = new ExpressionFieldDefinition<TDocument>(expression);
            var field = fieldDefinition.Render(_collection.DocumentSerializer, _collection.Settings.SerializerRegistry);
            _document[field.FieldName] = weight;
            return this;
        }
        
        public static implicit operator BsonDocument(WeightsBuilder<TDocument> source)
        {
            return source._document;
        }
    }
}
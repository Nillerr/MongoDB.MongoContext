using System;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public static class MongoSetExtensions
    {
        public static IFindFluent<TDocument> Find<TDocument>(this IMongoSet<TDocument> collection)
        {
            return collection.Find(FilterDefinition<TDocument>.Empty);
        }

        public static IFindFluent<TDocument> Find<TDocument>(
            this IMongoSet<TDocument> collection,
            Expression<Func<TDocument, bool>> expression)
        {
            return collection.Find(new ExpressionFilterDefinition<TDocument>(expression));
        }
    }
}
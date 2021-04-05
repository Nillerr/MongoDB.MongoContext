using System;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public static class FindFluentLinqExtensions
    {
        public static IFindFluent<TDocument> Where<TDocument>(
            this IFindFluentSource<TDocument> source,
            Expression<Func<TDocument, bool>> field)
        {
            var find = source.ToFindFluent();
            var fb = Builders<TDocument>.Filter;
            find.Filter = fb.And(find.Filter, fb.Where(field));
            return find;
        }
        
        public static IOrderedFindFluent<TDocument> OrderBy<TDocument>(
            this IFindFluentSource<TDocument> source,
            Expression<Func<TDocument, object>> field)
        {
            var find = source.ToFindFluent();
            return find.SortBy(field);
        }
        
        public static IOrderedFindFluent<TDocument> OrderByDescending<TDocument>(
            this IFindFluentSource<TDocument> source,
            Expression<Func<TDocument, object>> field)
        {
            var find = source.ToFindFluent();
            return find.SortByDescending(field);
        }
    }
}
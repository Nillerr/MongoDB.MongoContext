using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public static class FindFluentExtensions
    {
        public static IOrderedFindFluent<TDocument> SortBy<TDocument>(
            this IFindFluent<TDocument> find,
            Expression<Func<TDocument, object>> field)
        {
            var sort = Builders<TDocument>.Sort.Ascending(field);
            return find.Sort(sort);
        }

        public static IOrderedFindFluent<TDocument> SortByDescending<TDocument>(
            this IFindFluent<TDocument> find,
            Expression<Func<TDocument, object>> field)
        {
            var sort = Builders<TDocument>.Sort.Descending(field);
            return find.Sort(sort);
        }

        public static IOrderedFindFluent<TDocument> ThenBy<TDocument>(
            this IFindFluent<TDocument> find,
            Expression<Func<TDocument, object>> field)
        {
            var sb = Builders<TDocument>.Sort;
            var sort = sb.Combine(find.Options.Sort, sb.Ascending(field));
            return find.Sort(sort);
        }

        public static IOrderedFindFluent<TDocument> ThenByDescending<TDocument>(
            this IFindFluent<TDocument> find,
            Expression<Func<TDocument, object>> field)
        {
            var sb = Builders<TDocument>.Sort;
            var sort = sb.Combine(find.Options.Sort, sb.Descending(field));
            return find.Sort(sort);
        }

        public static async Task<TDocument> FirstAsync<TDocument>(
            this IFindFluent<TDocument> find,
            CancellationToken cancellationToken = default)
        {
            var cursor = await find.Limit(1).ToCursorAsync(cancellationToken);

            await cursor.MoveNextAsync(cancellationToken);
            return cursor.Current.First();
        }

        public static async Task<TDocument?> FirstOrDefaultAsync<TDocument>(
            this IFindFluent<TDocument> find,
            CancellationToken cancellationToken = default)
        {
            var cursor = await find.Limit(1).ToCursorAsync(cancellationToken);

            await cursor.MoveNextAsync(cancellationToken);
            return cursor.Current.FirstOrDefault();
        }

        public static async Task<TDocument> SingleAsync<TDocument>(
            this IFindFluent<TDocument> find,
            CancellationToken cancellationToken = default)
        {
            IAsyncCursor<TDocument> cursor;
            
            if (!find.Options.Limit.HasValue || find.Options.Limit.Value > 2)
            {
                cursor = await find.Limit(2).ToCursorAsync(cancellationToken);
            }
            else
            {
                cursor = await find.ToCursorAsync(cancellationToken);
            }

            await cursor.MoveNextAsync(cancellationToken);
            return cursor.Current.Single();
        }

        public static async Task<TDocument?> SingleOrDefaultAsync<TDocument>(
            this IFindFluent<TDocument> find,
            CancellationToken cancellationToken = default)
        {
            IAsyncCursor<TDocument> cursor;
            
            if (!find.Options.Limit.HasValue || find.Options.Limit.Value > 2)
            {
                cursor = await find.Limit(2).ToCursorAsync(cancellationToken);
            }
            else
            {
                cursor = await find.ToCursorAsync(cancellationToken);
            }

            await cursor.MoveNextAsync(cancellationToken);
            return cursor.Current.SingleOrDefault();
        }

        public static async Task<List<TDocument>> ToListAsync<TDocument>(
            this IFindFluent<TDocument> find,
            CancellationToken cancellationToken = default)
        {
            var cursor = await find.ToCursorAsync(cancellationToken);

            var result = new List<TDocument>();
            
            while (await cursor.MoveNextAsync(cancellationToken))
            {
                result.AddRange(cursor.Current);
            }
            
            return result;
        }
    }
}
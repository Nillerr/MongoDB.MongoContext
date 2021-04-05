using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public interface IFindFluent<TDocument> : IAsyncCursorSource<TDocument>, IFindFluentSource<TDocument>
    {
        FilterDefinition<TDocument> Filter { get; set; }
        FindOptions<TDocument, TDocument> Options { get; }

        Task<long> CountAsync(CancellationToken cancellationToken = default);
        IFindFluent<TDocument> Limit(int? limit);
        IFindFluent<TDocument> Skip(int? skip);
        IOrderedFindFluent<TDocument> Sort(SortDefinition<TDocument> sort);

        /// <summary>
        /// Returns the underlying <see cref="IFindFluent{TDocument,TProjection}"/> interface from MongoDB, disabling
        /// document tracking for documents returned by queries using the returned object.
        /// </summary>
        /// <returns>The MongoDB fluent query interface.</returns>
        IFindFluent<TDocument, TDocument> AsNoTracking();
    }
}
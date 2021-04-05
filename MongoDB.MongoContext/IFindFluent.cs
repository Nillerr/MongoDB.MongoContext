using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public interface IFindFluent<TDocument> : IAsyncCursorSource<TDocument>
    {
        FilterDefinition<TDocument> Filter { get; set; }
        FindOptions<TDocument, TDocument> Options { get; }

        Task<long> CountAsync(CancellationToken cancellationToken = default);
        IFindFluent<TDocument> Limit(int? limit);
        IFindFluent<TDocument> Skip(int? skip);
        IOrderedFindFluent<TDocument> Sort(SortDefinition<TDocument> sort);

        IFindFluent<TDocument, TDocument> AsNoTracking();
    }
}
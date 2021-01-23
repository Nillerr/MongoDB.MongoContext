using System;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDB.MongoContext
{
    /// <summary>
    /// A database transaction.
    /// </summary>
    public interface IMongoTransaction :
        IDisposable
#if NETSTANDARD2_1
        , IAsyncDisposable
#endif
    {
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task CommitAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Aborts the transaction.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task AbortAsync(CancellationToken cancellationToken = default);
    }
}
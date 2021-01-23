using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    internal sealed class MongoTransaction : IMongoTransaction
    {
        private readonly IClientSessionHandle _session;
        
        internal MongoTransaction(IClientSessionHandle session)
        {
            _session = session;
        }

        /// <inheritdoc />
        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _session.CommitTransactionAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task AbortAsync(CancellationToken cancellationToken = default)
        {
            await _session.AbortTransactionAsync(cancellationToken).ConfigureAwait(false);
        }
        
#if NETSTANDARD2_1
        public async ValueTask DisposeAsync()
        {
            if (_session.IsInTransaction)
            {
                await _session.AbortTransactionAsync().ConfigureAwait(false);
            }
        }
#endif

        public void Dispose()
        {
            if (_session.IsInTransaction)
            {
                _session.AbortTransaction();
            }
        }
    }
}
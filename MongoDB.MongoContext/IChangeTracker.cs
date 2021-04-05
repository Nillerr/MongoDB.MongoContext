using System.Threading;
using System.Threading.Tasks;

namespace MongoDB.MongoContext
{
    internal interface IChangeTracker
    {
        Task<AsyncDelegate> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
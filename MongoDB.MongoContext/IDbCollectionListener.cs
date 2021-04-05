using System.Threading;
using System.Threading.Tasks;

namespace MongoDB.MongoContext
{
    public interface IDbCollectionListener<TDocument>
    {
        Task OnEventsSavedAsync(
            PendingEventsSavedContext<TDocument> context,
            CancellationToken cancellationToken = default);
    }
}
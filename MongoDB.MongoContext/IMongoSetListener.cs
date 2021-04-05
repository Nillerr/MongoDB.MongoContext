using System.Threading;
using System.Threading.Tasks;

namespace MongoDB.MongoContext
{
    public interface IMongoSetListener<TDocument>
    {
        Task OnEventsSavedAsync(
            ChangesSavedContext<TDocument> context,
            CancellationToken cancellationToken = default);
    }
}
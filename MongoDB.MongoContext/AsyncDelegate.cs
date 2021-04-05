using System.Threading;
using System.Threading.Tasks;

namespace MongoDB.MongoContext
{
    internal delegate Task AsyncDelegate(CancellationToken cancellationToken = default);
}
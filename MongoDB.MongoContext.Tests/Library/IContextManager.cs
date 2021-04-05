using System.Threading.Tasks;

namespace MongoDB.MongoContext.Tests
{
    public interface IContextManager
    {
        Task DestroyContext(string name);
    }
}
using System.Threading.Tasks;

namespace MongoDB.MongoContext.Tests
{
    public interface IArticlesContextManager
    {
        Task DestroyContext(string name);
    }
}
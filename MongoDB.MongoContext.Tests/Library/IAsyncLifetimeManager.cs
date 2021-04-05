using Xunit;

namespace MongoDB.MongoContext.Tests
{
    public interface IAsyncLifetimeManager
    {
        void AddAsyncLifetime(IAsyncLifetime obj);
    }
}
using System;

namespace MongoDB.MongoContext.Tests
{
    public interface IAsyncDisposableManager
    {
        void AddAsyncDisposable(IAsyncDisposable disposable);
    }
}
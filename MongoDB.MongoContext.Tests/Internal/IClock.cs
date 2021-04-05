using System;

namespace MongoDB.MongoContext.Tests
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
using System.Collections.Generic;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public interface IMongoAggregate<TAggregate>
        where TAggregate : IMongoAggregate<TAggregate>
    {
        IReadOnlyCollection<UpdateDefinition<TAggregate>> DequeueUpdates();
    }
}
using System.Collections.Generic;

namespace MongoDB.MongoContext
{
    public interface IMongoAggregate<TAggregate>
        where TAggregate : IMongoAggregate<TAggregate>
    {
        IReadOnlyCollection<IPendingEvent<TAggregate>> DequeuePendingEvents();
    }
}
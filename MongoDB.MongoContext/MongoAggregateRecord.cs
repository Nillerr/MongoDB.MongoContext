using System;
using System.Collections.Generic;
using System.Threading;

namespace MongoDB.MongoContext
{
    public abstract record MongoAggregateRecord<TAggregate> : IMongoAggregate<TAggregate>
        where TAggregate : MongoAggregateRecord<TAggregate>
    {
        private List<IMutation<TAggregate>> _pendingMutations = new();

        protected void Append<TMutation>(TMutation e, Action<TMutation> apply)
            where TMutation : IMutation<TAggregate>
        {
            apply(e);
            _pendingMutations.Add(e);
        }
        
        IReadOnlyCollection<IMutation<TAggregate>> IMongoAggregate<TAggregate>.DequeueMutations()
        {
            return Interlocked.Exchange(ref _pendingMutations, new List<IMutation<TAggregate>>());
        }
    }
}
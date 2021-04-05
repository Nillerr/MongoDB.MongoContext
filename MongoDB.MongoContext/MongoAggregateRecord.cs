using System;
using System.Collections.Generic;
using System.Threading;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public abstract record MongoAggregateRecord<TAggregate> : IMongoAggregate<TAggregate>
        where TAggregate : MongoAggregateRecord<TAggregate>
    {
        private List<UpdateDefinition<TAggregate>> _pendingMutations = new();

        protected void Update(Func<UpdateDefinitionBuilder<TAggregate>, UpdateDefinition<TAggregate>> selector)
        {
            _pendingMutations.Add(selector(Builders<TAggregate>.Update));
        }
        
        IReadOnlyCollection<UpdateDefinition<TAggregate>> IMongoAggregate<TAggregate>.DequeueUpdates()
        {
            return Interlocked.Exchange(ref _pendingMutations, new List<UpdateDefinition<TAggregate>>());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public abstract class MongoAggregate<TAggregate> : IMongoAggregate<TAggregate>, IEquatable<MongoAggregate<TAggregate>> where TAggregate : MongoAggregate<TAggregate>
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

        public bool Equals(MongoAggregate<TAggregate>? other)
        {
            return other != null;
        }

        public override bool Equals(object? obj)
        {
            return ReferenceEquals(this, obj) || obj is MongoAggregate<TAggregate> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
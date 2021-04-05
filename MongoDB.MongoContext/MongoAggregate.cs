using System;
using System.Collections.Generic;
using System.Threading;

namespace MongoDB.MongoContext
{
    public abstract class MongoAggregate<TAggregate> : IMongoAggregate<TAggregate>, IEquatable<MongoAggregate<TAggregate>> where TAggregate : MongoAggregate<TAggregate>
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
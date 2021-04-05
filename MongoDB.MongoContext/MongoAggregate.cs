using System;
using System.Collections.Generic;
using System.Threading;

namespace MongoDB.MongoContext
{
    public abstract class MongoAggregate<TAggregate> : IMongoAggregate<TAggregate>, IEquatable<MongoAggregate<TAggregate>> where TAggregate : MongoAggregate<TAggregate>
    {
        private List<IPendingEvent<TAggregate>> _pendingEvents = new();

        protected void Append<TEvent>(TEvent e, Action<TEvent> apply)
            where TEvent : IPendingEvent<TAggregate>
        {
            apply(e);
            _pendingEvents.Add(e);
        }
        
        public IReadOnlyCollection<IPendingEvent<TAggregate>> DequeuePendingEvents()
        {
            return Interlocked.Exchange(ref _pendingEvents, new List<IPendingEvent<TAggregate>>());
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
using System;
using System.Collections.Generic;
using System.Threading;

namespace MongoDB.MongoContext
{
    public abstract record MongoAggregateRecord<TAggregate> : IMongoAggregate<TAggregate>
        where TAggregate : MongoAggregateRecord<TAggregate>
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
    }
}
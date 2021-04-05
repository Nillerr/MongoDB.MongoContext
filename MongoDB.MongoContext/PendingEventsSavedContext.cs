using System.Collections.Generic;

namespace MongoDB.MongoContext
{
    public sealed class PendingEventsSavedContext<TDocument>
    {
        public PendingEventsSavedContext(IReadOnlyCollection<IPendingEvent<TDocument>> pendingEvents)
        {
            PendingEvents = pendingEvents;
        }

        public IReadOnlyCollection<IPendingEvent<TDocument>> PendingEvents { get; }
    }
}
using System.Collections.Generic;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    internal sealed class SaveDocumentsChangesContext<TDocument>
        where TDocument : IMongoAggregate<TDocument>
    {
        private readonly List<IPendingEvent<TDocument>> _pendingEvents = new();
        private readonly List<WriteModel<TDocument>> _writeModels = new();

        public IReadOnlyList<IPendingEvent<TDocument>> PendingEvents => _pendingEvents;
        public IReadOnlyList<WriteModel<TDocument>> WriteModels => _writeModels;

        public void AddPendingEvents(IReadOnlyCollection<IPendingEvent<TDocument>> pendingEvents)
        {
            _pendingEvents.AddRange(pendingEvents);
        }

        public void AddWriteModel(WriteModel<TDocument> writeModel)
        {
            _writeModels.Add(writeModel);
        }
    }
}
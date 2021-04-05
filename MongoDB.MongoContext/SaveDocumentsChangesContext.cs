using System.Collections.Generic;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    internal sealed class SaveDocumentsChangesContext<TDocument>
        where TDocument : IMongoAggregate<TDocument>
    {
        private readonly List<UpdateDefinition<TDocument>> _updates = new();
        private readonly List<WriteModel<TDocument>> _writeModels = new();

        public IReadOnlyList<UpdateDefinition<TDocument>> Updates => _updates;
        public IReadOnlyList<WriteModel<TDocument>> WriteModels => _writeModels;

        public void AddUpdates(IReadOnlyCollection<UpdateDefinition<TDocument>> updates)
        {
            _updates.AddRange(updates);
        }

        public void AddWriteModel(WriteModel<TDocument> writeModel)
        {
            _writeModels.Add(writeModel);
        }
    }
}
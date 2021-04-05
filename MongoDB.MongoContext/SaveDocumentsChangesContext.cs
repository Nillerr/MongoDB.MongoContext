using System.Collections.Generic;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    internal sealed class SaveDocumentsChangesContext<TDocument>
        where TDocument : IMongoAggregate<TDocument>
    {
        private readonly List<IMutation<TDocument>> _mutations = new();
        private readonly List<WriteModel<TDocument>> _writeModels = new();

        public IReadOnlyList<IMutation<TDocument>> Mutations => _mutations;
        public IReadOnlyList<WriteModel<TDocument>> WriteModels => _writeModels;

        public void AddMutations(IReadOnlyCollection<IMutation<TDocument>> mutations)
        {
            _mutations.AddRange(mutations);
        }

        public void AddWriteModel(WriteModel<TDocument> writeModel)
        {
            _writeModels.Add(writeModel);
        }
    }
}
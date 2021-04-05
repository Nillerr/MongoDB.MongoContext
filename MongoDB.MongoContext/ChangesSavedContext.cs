using System.Collections.Generic;

namespace MongoDB.MongoContext
{
    public sealed class ChangesSavedContext<TDocument>
    {
        public ChangesSavedContext(IReadOnlyCollection<IMutation<TDocument>> mutations)
        {
            Mutations = mutations;
        }

        public IReadOnlyCollection<IMutation<TDocument>> Mutations { get; }
    }
}
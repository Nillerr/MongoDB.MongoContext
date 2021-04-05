using System.Collections.Generic;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public sealed class ChangesSavedContext<TDocument>
    {
        public ChangesSavedContext(IReadOnlyCollection<UpdateDefinition<TDocument>> updates)
        {
            Updates = updates;
        }

        public IReadOnlyCollection<UpdateDefinition<TDocument>> Updates { get; }
    }
}
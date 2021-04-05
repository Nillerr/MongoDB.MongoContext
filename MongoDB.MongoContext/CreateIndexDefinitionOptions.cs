using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public class CreateIndexDefinitionOptions<TDocument> : CreateIndexOptions<TDocument>
    {
        public IndexOptionsConflictResolution IndexOptionsConflictResolution { get; set; } =
            IndexOptionsConflictResolution.None;
    }
}
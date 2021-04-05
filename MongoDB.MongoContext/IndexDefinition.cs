using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public class IndexDefinition<TDocument>
    {
        public IndexDefinition(
            IndexKeysDefinition<TDocument> keys,
            CreateIndexDefinitionOptions<TDocument> options)
        {
            Model = new CreateIndexModel<TDocument>(keys, options);
            IndexOptionsConflictResolution = options.IndexOptionsConflictResolution;
        }

        public CreateIndexModel<TDocument> Model { get; }
        public IndexOptionsConflictResolution IndexOptionsConflictResolution { get; }
    }
}
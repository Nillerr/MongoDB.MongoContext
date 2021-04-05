using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public delegate FilterDefinition<TDocument> PrimaryKeyFilterSelector<TDocument>(
        FilterDefinitionBuilder<TDocument> filter,
        TDocument document);
}
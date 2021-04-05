using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public interface IPendingEvent<T>
    {
        UpdateDefinition<T> ToUpdateDefinition(UpdateDefinitionBuilder<T> update);
    }
}
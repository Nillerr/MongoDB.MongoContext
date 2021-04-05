using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public interface IMutation<T>
    {
        UpdateDefinition<T> ToUpdateDefinition(UpdateDefinitionBuilder<T> update);
    }
}
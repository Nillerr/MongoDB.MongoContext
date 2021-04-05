using MongoDB.Driver;

namespace MongoDB.MongoContext.Tests
{
internal sealed record ArticleTitleChanged(string Title) : IMutation<Article>
{
    public UpdateDefinition<Article> ToUpdateDefinition(UpdateDefinitionBuilder<Article> update)
    {
        return update.Set(e => e.Title, Title);
    }
}
}
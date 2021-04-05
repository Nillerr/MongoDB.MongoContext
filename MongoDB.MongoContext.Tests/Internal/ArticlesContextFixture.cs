using System.Threading.Tasks;
using Xunit;

namespace MongoDB.MongoContext.Tests
{
    public sealed class ArticlesContextFixture : IAsyncLifetime
    {
        private readonly IArticlesContextManager _contextManager;

        public ArticlesContextFixture(IArticlesContextManager contextManager, string name, ArticlesContext context)
        {
            _contextManager = contextManager;
            Name = name;
            Context = context;
        }

        public string Name { get; }
        public ArticlesContext Context { get; }

        public Task InitializeAsync()
        {
            return Context.InitializeAsync();
        }

        public Task DisposeAsync()
        {
            return _contextManager.DestroyContext(Name);
        }
    }
}
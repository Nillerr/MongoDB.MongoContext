using System;
using System.Threading.Tasks;

namespace MongoDB.MongoContext.Tests
{
    public sealed class ArticlesContextFixture : IAsyncDisposable
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
        
        public async ValueTask DisposeAsync()
        {
            await _contextManager.DestroyContext(Name);
        }
    }
}
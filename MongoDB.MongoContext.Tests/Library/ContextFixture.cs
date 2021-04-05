using System.Threading.Tasks;
using Xunit;

namespace MongoDB.MongoContext.Tests
{
    public sealed class ContextFixture<TContext> : IAsyncLifetime
        where TContext : MongoContext
    {
        private readonly IContextManager _contextManager;

        public ContextFixture(IContextManager contextManager, string name, TContext context)
        {
            _contextManager = contextManager;
            Name = name;
            Context = context;
        }

        public string Name { get; }
        
        public TContext Context { get; }

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
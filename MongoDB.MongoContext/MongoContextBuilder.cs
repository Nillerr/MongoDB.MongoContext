using Microsoft.Extensions.DependencyInjection;

namespace MongoDB.MongoContext
{
    internal sealed class MongoContextBuilder : IMongoContextBuilder
    {
        public MongoContextBuilder(MongoContextOptions options, IServiceCollection services)
        {
            Options = options;
            Services = services;
        }

        public MongoContextOptions Options { get; }
        public IServiceCollection Services { get; }
    }
}
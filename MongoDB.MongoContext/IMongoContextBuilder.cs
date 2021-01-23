using Microsoft.Extensions.DependencyInjection;

namespace MongoDB.MongoContext
{
    /// <summary>
    /// A builder object for configuring MongoDB contexts.
    /// </summary>
    public interface IMongoContextBuilder
    {
        /// <summary>
        /// The options of the context.
        /// </summary>
        MongoContextOptions Options { get; }
        
        /// <summary>
        /// The underlying <see cref="IServiceCollection"/>.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
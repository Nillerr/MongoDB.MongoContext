using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    /// <summary>
    /// Extensions for adding MongoDB contexts to the a <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the MongoDB context specified by <typeparamref name="TContext"/> to the
        /// <see cref="IServiceCollection"/> using the <paramref name="contextFactory"/>, connecting to the MongoDB
        /// server and database using the specified <paramref name="connectionString"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="connectionString">The connection string, including a database name.</param>
        /// <param name="configure">An optional configuration function.</param>
        /// <typeparam name="TContext">The type of context.</typeparam>
        /// <returns>The MongoDB context builder for the given context.</returns>
        public static IMongoContextBuilder AddMongoContext<TContext>(
            this IServiceCollection services,
            Func<IServiceProvider, MongoContextOptions, TContext> contextFactory,
            string connectionString,
#if NETSTANDARD2_1
            Action<MongoContextOptions>? configure = null
#else
            Action<MongoContextOptions> configure = null
#endif
        )
            where TContext : MongoContext
        {
            var url = MongoUrl.Create(connectionString);
            return services.AddMongoContext(contextFactory, url, configure);
        }
        
        /// <summary>
        /// Adds the MongoDB context specified by <typeparamref name="TContext"/> to the
        /// <see cref="IServiceCollection"/> using the <paramref name="contextFactory"/>, connecting to the MongoDB
        /// server and database using the specified <paramref name="url"/> as the connection string.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="url">The connection string, including a database name.</param>
        /// <param name="configure">An optional configuration function.</param>
        /// <typeparam name="TContext">The type of context.</typeparam>
        /// <returns>The MongoDB context builder for the given context.</returns>
        public static IMongoContextBuilder AddMongoContext<TContext>(
            this IServiceCollection services,
            Func<IServiceProvider, MongoContextOptions, TContext> contextFactory,
            MongoUrl url,
#if NETSTANDARD2_1
            Action<MongoContextOptions>? configure = null
#else
            Action<MongoContextOptions> configure = null
#endif
        )
            where TContext : MongoContext
        {
            var client = new MongoClient(url);
            
            var databaseName = url.DatabaseName;
            if (databaseName is null)
            {
                throw new ArgumentException("The connection string / url must specify a default database.", nameof(url));
            }
            
            var database = client.GetDatabase(databaseName);
            return services.AddMongoContext(contextFactory, database, configure);
        }
        
        /// <summary>
        /// Adds the MongoDB context specified by <typeparamref name="TContext"/> to the
        /// <see cref="IServiceCollection"/> using the <paramref name="contextFactory"/>, connecting to the specified
        /// MongoDB <paramref name="database"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="database">The <see cref="IMongoDatabase"/> the context connects to.</param>
        /// <param name="configure">An optional configuration function.</param>
        /// <typeparam name="TContext">The type of context.</typeparam>
        /// <returns>The MongoDB context builder for the given context.</returns>
        public static IMongoContextBuilder AddMongoContext<TContext>(
            this IServiceCollection services,
            Func<IServiceProvider, MongoContextOptions, TContext> contextFactory,
            IMongoDatabase database,
#if NETSTANDARD2_1
            Action<MongoContextOptions>? configure = null
#else
            Action<MongoContextOptions> configure = null
#endif
        )
            where TContext : MongoContext
        {
            var options = new MongoContextOptions(database);
            configure?.Invoke(options);
            
            services.AddScoped(serviceProvider => contextFactory(serviceProvider, options));
            return new MongoContextBuilder(options, services);
        }
    }
}
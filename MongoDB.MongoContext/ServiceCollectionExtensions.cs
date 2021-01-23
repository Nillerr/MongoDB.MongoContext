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
            Func<MongoContextOptions, TContext> contextFactory,
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
            Func<MongoContextOptions, TContext> contextFactory,
            MongoUrl url,
#if NETSTANDARD2_1
            Action<MongoContextOptions>? configure = null
#else
            Action<MongoContextOptions> configure = null
#endif
        )
            where TContext : MongoContext
        {
            var database = GetDatabase(url);
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
            Func<MongoContextOptions, TContext> contextFactory,
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

            return services.AddMongoContext(contextFactory, options);
        }
        
        /// <summary>
        /// Adds the MongoDB context specified by <typeparamref name="TContext"/> to the
        /// <see cref="IServiceCollection"/> using the <paramref name="contextFactory"/>, using the specified
        /// <see cref="MongoContextOptions"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="options">The mongo context options</param>
        /// <typeparam name="TContext">The type of context.</typeparam>
        /// <returns>The MongoDB context builder for the given context.</returns>
        public static IMongoContextBuilder AddMongoContext<TContext>(
            this IServiceCollection services,
            Func<MongoContextOptions, TContext> contextFactory,
            MongoContextOptions options
        )
            where TContext : MongoContext
        {
            services.AddScoped(serviceProvider => contextFactory(options));
            
            return new MongoContextBuilder(options, services);
        }

        /// <summary>
        /// Adds the MongoDB context specified by <typeparamref name="TContext"/> to the
        /// <see cref="IServiceCollection"/> using the <typeparamref name="TContextFactory"/>, connecting to the MongoDB
        /// server and database using the specified <paramref name="connectionString"/> as the connection string.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="connectionString">The connection string, including a database name.</param>
        /// <param name="configure">An optional configuration function.</param>
        /// <typeparam name="TContext">The type of context.</typeparam>
        /// <typeparam name="TContextFactory">The type of context factory.</typeparam>
        /// <returns>The MongoDB context builder for the given context.</returns>
        public static IMongoContextBuilder AddMongoContext<TContext, TContextFactory>(
            this IServiceCollection services,
            string connectionString,
#if NETSTANDARD2_1
            Action<MongoContextOptions>? configure = null
#else
            Action<MongoContextOptions> configure = null
#endif
        )
            where TContext : MongoContext
            where TContextFactory : class, IMongoContextFactory<TContext>
        {
            var url = new MongoUrl(connectionString);
            return services.AddMongoContext<TContext, TContextFactory>(url, configure);
        }

        /// <summary>
        /// Adds the MongoDB context specified by <typeparamref name="TContext"/> to the
        /// <see cref="IServiceCollection"/> using the <typeparamref name="TContextFactory"/>, connecting to the MongoDB
        /// server and database using the specified <paramref name="url"/> as the connection string.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="url">The connection string, including a database name.</param>
        /// <param name="configure">An optional configuration function.</param>
        /// <typeparam name="TContext">The type of context.</typeparam>
        /// <typeparam name="TContextFactory">The type of context factory.</typeparam>
        /// <returns>The MongoDB context builder for the given context.</returns>
        public static IMongoContextBuilder AddMongoContext<TContext, TContextFactory>(
            this IServiceCollection services,
            MongoUrl url,
#if NETSTANDARD2_1
            Action<MongoContextOptions>? configure = null
#else
            Action<MongoContextOptions> configure = null
#endif
        )
            where TContext : MongoContext
            where TContextFactory : class, IMongoContextFactory<TContext>
        {
            var database = GetDatabase(url);
            return services.AddMongoContext<TContext, TContextFactory>(database, configure);
        }

        private static IMongoDatabase GetDatabase(MongoUrl url)
        {
            var client = new MongoClient(url);

            var databaseName = url.DatabaseName;
            if (databaseName is null)
            {
                throw new ArgumentException("The connection string / url must specify a default database.", nameof(url));
            }

            var database = client.GetDatabase(databaseName);
            return database;
        }

        /// <summary>
        /// Adds the MongoDB context specified by <typeparamref name="TContext"/> to the
        /// <see cref="IServiceCollection"/> using the <typeparamref name="TContextFactory"/>, connecting to the specified
        /// MongoDB <paramref name="database"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="database">The <see cref="IMongoDatabase"/> the context connects to.</param>
        /// <param name="configure">An optional configuration function.</param>
        /// <typeparam name="TContext">The type of context.</typeparam>
        /// <typeparam name="TContextFactory">The type of context factory.</typeparam>
        /// <returns>The MongoDB context builder for the given context.</returns>
        public static IMongoContextBuilder AddMongoContext<TContext, TContextFactory>(
            this IServiceCollection services,
            IMongoDatabase database,
#if NETSTANDARD2_1
            Action<MongoContextOptions>? configure = null
#else
            Action<MongoContextOptions> configure = null
#endif
        )
            where TContext : MongoContext
            where TContextFactory : class, IMongoContextFactory<TContext>
        {
            var options = new MongoContextOptions(database);
            configure?.Invoke(options);

            return services.AddMongoContext<TContext, TContextFactory>(options);
        }

        /// <summary>
        /// Adds the MongoDB context specified by <typeparamref name="TContext"/> to the
        /// <see cref="IServiceCollection"/> using the <typeparamref name="TContextFactory"/>, using the specified
        /// <see cref="MongoContextOptions"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="options">The mongo context options</param>
        /// <typeparam name="TContext">The type of context.</typeparam>
        /// <typeparam name="TContextFactory">The type of context factory.</typeparam>
        /// <returns>The MongoDB context builder for the given context.</returns>
        public static IMongoContextBuilder AddMongoContext<TContext, TContextFactory>(
            this IServiceCollection services,
            MongoContextOptions options
        )
            where TContext : MongoContext
            where TContextFactory : class, IMongoContextFactory<TContext>
        {
            services.AddScoped<TContextFactory>();
            services.AddScoped(serviceProvider => serviceProvider
                .GetRequiredService<TContextFactory>()
                .CreateContext(options)
            );
            
            return new MongoContextBuilder(options, services);
        }

        /// <summary>
        /// Adds the MongoDB context specified by <typeparamref name="TContext"/> to the
        /// <see cref="IServiceCollection"/> using the <paramref name="contextFactory"/>, using the specified
        /// <see cref="MongoContextOptions"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="contextFactory">The context factory.</param>
        /// <param name="options">The mongo context options</param>
        /// <typeparam name="TContext">The type of context.</typeparam>
        /// <returns>The MongoDB context builder for the given context.</returns>
        public static IMongoContextBuilder AddMongoContext<TContext>(
            this IServiceCollection services,
            IMongoContextFactory<TContext> contextFactory,
            MongoContextOptions options
        )
            where TContext : MongoContext
        {
            services.AddScoped(serviceProvider => contextFactory.CreateContext(options));
            
            return new MongoContextBuilder(options, services);
        }
    }
}
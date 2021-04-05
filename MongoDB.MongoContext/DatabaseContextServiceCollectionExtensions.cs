using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public static class DatabaseContextServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoContext<T>(
            this IServiceCollection serviceCollection,
            IMongoDatabase database,
            Func<DatabaseContextOptions, T> contextFactory,
            Action<MongoContextOptions>? configure = null)
            where T : MongoContext
        {
            var options = new MongoContextOptions();
            configure?.Invoke(options);
            
            serviceCollection.AddScoped<T>(serviceProvider =>
            {
                var listenerFactories = options.Listeners
                    .Select(factory => factory(serviceProvider))
                    .ToList();
                
                var databaseOptions = new DatabaseContextOptions(database, listenerFactories);
                return contextFactory(databaseOptions);
            });

            return serviceCollection;
        }
    }
}
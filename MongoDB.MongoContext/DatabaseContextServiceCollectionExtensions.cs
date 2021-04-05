using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public static class DatabaseContextServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoContext<T>(
            this IServiceCollection serviceCollection,
            IMongoDatabase database,
            Func<DatabaseContextOptions, T> contextFactory)
            where T : MongoContext
        {
            serviceCollection.AddScoped<T>(serviceProvider =>
            {
                var databaseOptions = new DatabaseContextOptions(database);
                return contextFactory(databaseOptions);
            });

            return serviceCollection;
        }
    }
}
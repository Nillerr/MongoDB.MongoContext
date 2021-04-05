using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace MongoDB.MongoContext
{
    public sealed class MongoContextOptions
    {
        public delegate IDbCollectionListenerFactory ListenerFactory(IServiceProvider serviceProvider);

        public List<ListenerFactory> Listeners { get; } = new();

        public void AddListener(ListenerFactory listener)
        {
            Listeners.Add(listener);
        }

        public void AddListener<T>()
            where T : class, IDbCollectionListenerFactory
        {
            Listeners.Add(serviceProvider => ActivatorUtilities.CreateInstance<T>(serviceProvider));
        }
    }
}
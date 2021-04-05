using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MongoDB.MongoContext.Tests
{
    public abstract class Tests : IAsyncLifetime, IAsyncLifetimeManager
    {
        private readonly List<IAsyncLifetime> _asyncLifetimes = new();
        
        public void AddAsyncLifetime(IAsyncLifetime obj)
        {
            _asyncLifetimes.Add(obj);
        }

        public async Task InitializeAsync()
        {
            var exceptions = new List<Exception>();
            
            foreach (var asyncLifetime in _asyncLifetimes)
            {
                try
                {
                    await asyncLifetime.InitializeAsync();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            try
            {
                await OnInitializeAsync();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        public async Task DisposeAsync()
        {
            var exceptions = new List<Exception>();

            try
            {
                await OnDisposeAsync();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            
            foreach (var asyncLifetime in _asyncLifetimes)
            {
                try
                {
                    await asyncLifetime.DisposeAsync();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        protected virtual Task OnInitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnDisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
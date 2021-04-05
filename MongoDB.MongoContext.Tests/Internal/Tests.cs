using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MongoDB.MongoContext.Tests
{
    public abstract class Tests : IAsyncLifetime, IAsyncDisposableManager
    {
        private readonly List<IAsyncDisposable> _asyncDisposables = new();
        
        public void AddAsyncDisposable(IAsyncDisposable disposable)
        {
            _asyncDisposables.Add(disposable);
        }

        public Task InitializeAsync()
        {
            return OnInitializeAsync();
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
            
            foreach (var asyncDisposable in _asyncDisposables)
            {
                try
                {
                    await asyncDisposable.DisposeAsync();
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
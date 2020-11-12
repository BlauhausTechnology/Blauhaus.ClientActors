using System;
using System.Threading;
using System.Threading.Tasks; 

namespace Blauhaus.ClientActors
{
    public abstract class BaseSlimActor : IAsyncDisposable
    {
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1);
         
        protected virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected async Task DoAsync(Action action, CancellationToken cancellationToken = default)
        {
            await _lock.WaitAsync();
            try
            {
                action.Invoke();
            }
            finally
            {
                _lock.Release();
            }
        }

        protected async Task<T> DoAsync<T>(Func<T> function, CancellationToken cancellationToken = default)
        {
            await _lock.WaitAsync();
            try
            {
                return function.Invoke();
            }
            finally
            {
                _lock.Release();
            }
        }

        protected async Task DoAsync(Func<Task> asyncAction, CancellationToken cancellationToken = default)
        {
            await _lock.WaitAsync();
            try
            {
                await asyncAction.Invoke();
            }
            finally
            {
                _lock.Release();
            }
        }

        protected async Task<T> DoAsync<T>(Func<Task<T>> asyncFunction, CancellationToken cancellationToken = default) 
        {
            await _lock.WaitAsync();
            try
            {
                return await asyncFunction.Invoke();
            }
            finally
            {
                _lock.Release();
            }
        }
        
        protected virtual void Shutdown()
        {
        }

        public virtual ValueTask DisposeAsync()
        {
            _lock.Dispose();
            return new ValueTask();
        }
    }
}
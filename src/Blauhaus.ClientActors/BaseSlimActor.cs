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

        protected Task DoAsync(Action action, CancellationToken cancellationToken = default)
        {
            _lock.Wait();
            try
            {
                return Task.Run(action.Invoke);
            }
            finally
            {
                _lock.Release();
            }
        }

        protected Task<T> DoAsync<T>(Func<T> function, CancellationToken cancellationToken = default)
        {
            _lock.Wait();
            try
            {
                return Task.Run(function.Invoke);
            }
            finally
            {
                _lock.Release();
            }
        }

        protected Task DoAsync(Func<Task> asyncAction, CancellationToken cancellationToken = default)
        {
            _lock.Wait();
            try
            {
                return Task.Run(async ()=> await asyncAction.Invoke());
            }
            finally
            {
                _lock.Release();
            }
        }

        protected Task<T> DoAsync<T>(Func<Task<T>> asyncFunction, CancellationToken cancellationToken = default) 
        {
            _lock.Wait();
            try
            {
                return Task.Run(async ()=> await asyncFunction.Invoke());
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
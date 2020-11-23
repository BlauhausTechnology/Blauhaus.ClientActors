using System;
using System.Threading;
using System.Threading.Tasks;
using Winton.Extensions.Threading.Actor;

namespace Blauhaus.ClientActors
{
    public abstract class BaseActor : IAsyncDisposable
    {
        private readonly Actor? _handler;
        private readonly SemaphoreSlim _lock; 


        protected BaseActor()
        {
            _lock = new SemaphoreSlim(1);

            _handler = new Actor();

            _handler
                .WithStartWork(InitializeAsync)
                .WithStopWork(Shutdown);

            _handler.Start();
        }
         
         
        protected virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected Task DoAsync(Action action, CancellationToken cancellationToken = default) 
            => _handler.Enqueue(action.Invoke, cancellationToken);

        protected Task<T> DoAsync<T>(Func<T> function, CancellationToken cancellationToken = default) 
            => _handler.Enqueue(function.Invoke, cancellationToken);

        protected Task DoAsync(Func<Task> asyncAction, CancellationToken cancellationToken = default) 
            => _handler.Enqueue(async () => await asyncAction.Invoke(), cancellationToken);

        protected Task<T> DoAsync<T>(Func<Task<T>> asyncFunction, CancellationToken cancellationToken = default) 
            => _handler.Enqueue(async () => await asyncFunction.Invoke(), cancellationToken);

        protected Task DoAndBlockAsync(Action action)
        {
            return DoAsync(() =>
            {
                _lock.Wait();
                try
                {
                    action.Invoke();
                }
                finally
                {
                    _lock.Release();
                }
            });
        }

        protected Task<T> DoAndBlockAsync<T>(Func<T> function)
        {
            return DoAsync(() =>
            {
                _lock.Wait();
                try
                {
                    return function.Invoke();
                }
                finally
                {
                    _lock.Release();
                }
            });
             
        }
        
        protected Task DoAndBlockAsync(Func<Task> asyncAction)
        {
            return DoAsync(async () =>
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
            });
        }

        protected Task<T> DoAndBlockAsync<T>(Func<Task<T>> asyncFunction) 
        {
           return DoAsync(async () =>
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
           });
        }
        
        protected virtual void Shutdown()
        {
        }

        public virtual async ValueTask DisposeAsync()
        {
            await _handler.Stop();
        }
    }
}
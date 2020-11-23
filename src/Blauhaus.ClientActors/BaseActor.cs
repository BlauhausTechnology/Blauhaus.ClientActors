using System;
using System.Threading;
using System.Threading.Tasks;
using Winton.Extensions.Threading.Actor;
using IActor = Winton.Extensions.Threading.Actor.IActor;

namespace Blauhaus.ClientActors
{
    public abstract class BaseActor : IAsyncDisposable
    {
        private Actor? _backingHandler;
        private SemaphoreSlim? _lock;

        private SemaphoreSlim Lock => _lock ??= new SemaphoreSlim(1);

        private IActor Handler
        {
            get
            {
                if (_backingHandler == null)
                {
                    _backingHandler = new Actor();
                    
                    _backingHandler
                        .WithStartWork(InitializeAsync)
                        .WithStopWork(Shutdown);

                    Handler.Start();
                }

                return _backingHandler;
            }
        }
         
        protected virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected Task DoAsync(Action action, CancellationToken cancellationToken = default) 
            => Handler.Enqueue(action.Invoke, cancellationToken);

        protected Task<T> DoAsync<T>(Func<T> function, CancellationToken cancellationToken = default) 
            => Handler.Enqueue(function.Invoke, cancellationToken);

        protected Task DoAsync(Func<Task> asyncAction, CancellationToken cancellationToken = default) 
            => Handler.Enqueue(async () => await asyncAction.Invoke(), cancellationToken);

        protected Task<T> DoAsync<T>(Func<Task<T>> asyncFunction, CancellationToken cancellationToken = default) 
            => Handler.Enqueue(async () => await asyncFunction.Invoke(), cancellationToken);

        protected Task DoAndBlockAsync(Action action)
        {
            return DoAsync(() =>
            {
                Lock.Wait();
                try
                {
                    action.Invoke();
                }
                finally
                {
                    Lock.Release();
                }
            });
        }

        protected Task<T> DoAndBlockAsync<T>(Func<T> function)
        {
            return DoAsync(() =>
            {
                Lock.Wait();
                try
                {
                    return function.Invoke();
                }
                finally
                {
                    Lock.Release();
                }
            });
             
        }
        
        protected Task DoAndBlockAsync(Func<Task> asyncAction)
        {
            return DoAsync(async () =>
            {
                await Lock.WaitAsync();
                try
                {
                    await asyncAction.Invoke();
                }
                finally
                {
                    Lock.Release();
                }
            });
        }

        protected Task<T> DoAndBlockAsync<T>(Func<Task<T>> asyncFunction) 
        {
           return DoAsync(async () =>
           {
               await Lock.WaitAsync();
               try
               {
                   return await asyncFunction.Invoke();
               }
               finally
               {
                   Lock.Release();
               }
           });
        }
        
        protected virtual void Shutdown()
        {
        }

        public virtual async ValueTask DisposeAsync()
        {
            await Handler.Stop();
        }
    }
}
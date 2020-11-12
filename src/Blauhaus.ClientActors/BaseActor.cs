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
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1);

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

        protected Task DoAsync(Action action, CancellationToken cancellationToken = default) => Handler.Enqueue(() =>
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
        }, cancellationToken);

        protected Task<T> DoAsync<T>(Func<T> function, CancellationToken cancellationToken = default) => Handler.Enqueue(() =>
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
        }, cancellationToken);

        protected Task DoAsync(Func<Task> asyncAction, CancellationToken cancellationToken = default) => Handler.Enqueue(async () =>
        {
            _lock.Wait();
            try
            {
                await asyncAction.Invoke();
            }
            finally
            {
                _lock.Release();
            }
        }, cancellationToken);

        protected Task<T> DoAsync<T>(Func<Task<T>> asyncFunction, CancellationToken cancellationToken = default) => Handler.Enqueue(async () =>
        {
            _lock.Wait();
            try
            {
                return await asyncFunction.Invoke();
            }
            finally
            {
                _lock.Release();
            }
        }, cancellationToken);

        
        protected virtual void Shutdown()
        {
        }

        public virtual async ValueTask DisposeAsync()
        {
            await Handler.Stop();
            _lock.Dispose();
        }
    }
}
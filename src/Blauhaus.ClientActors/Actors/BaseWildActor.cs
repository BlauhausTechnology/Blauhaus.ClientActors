using System;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Common.Utils.Disposables;
using Winton.Extensions.Threading.Actor;

namespace Blauhaus.ClientActors.Actors
{
    public abstract class BaseWildActor : BasePublisher, IAsyncDisposable
    {
        private readonly Actor _handler;
        private readonly SemaphoreSlim _lock; 

        protected BaseWildActor()
        {
            _lock = new SemaphoreSlim(1);

            _handler = new Actor();
            _handler
                .WithStartWork(OnStartUpAsync)
                .WithStopWork(Shutdown);
            _handler.Start();
        }

        protected virtual Task OnStartUpAsync()
        {
            return Task.CompletedTask;
        }

        protected Task InvokeAsync(Action action, CancellationToken cancellationToken = default) 
            => _handler.Enqueue(action.Invoke, cancellationToken);

        protected Task<T> InvokeAsync<T>(Func<T> function, CancellationToken cancellationToken = default) 
            => _handler.Enqueue(function.Invoke, cancellationToken);

        protected Task InvokeAsync(Func<Task> asyncAction, CancellationToken cancellationToken = default) 
            => _handler.Enqueue(async () => await asyncAction.Invoke(), cancellationToken);

        protected Task<T> InvokeAsync<T>(Func<Task<T>> asyncFunction, CancellationToken cancellationToken = default) 
            => _handler.Enqueue(async () => await asyncFunction.Invoke(), cancellationToken);
         
        protected Task InvokeLockedAsync(Action action)
        {
            return InvokeAsync(() =>
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

        protected Task<T> InvokeLockedAsync<T>(Func<T> function)
        {
            return InvokeAsync(() =>
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
        
        protected Task InvokeLockedAsync(Func<Task> asyncAction)
        {
            return InvokeAsync(async () =>
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

        protected Task<T> InvokeLockedAsync<T>(Func<Task<T>> asyncFunction) 
        {
            return InvokeAsync(async () =>
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
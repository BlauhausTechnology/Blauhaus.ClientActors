using System;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Common.Utils.Disposables;
using Winton.Extensions.Threading.Actor;

namespace Blauhaus.ClientActors.Actors
{
    public abstract class BaseActor : BasePublisher, IAsyncDisposable
    {
        private readonly Actor _handler;
        private readonly SemaphoreSlim _lock; 
        

        protected BaseActor()
        {
            _lock = new SemaphoreSlim(1);

            _handler = new Actor();
            _handler
                .WithStopWork(Shutdown);
            _handler.Start();
        }
          
        protected Task InvokeAsync(Action action, CancellationToken cancellationToken = default) 
            => _handler.Enqueue(action.Invoke, cancellationToken);

        protected Task<T> InvokeAsync<T>(Func<T> function, CancellationToken cancellationToken = default) 
            => _handler.Enqueue(function.Invoke, cancellationToken);

        protected Task InvokeAsync(Func<Task> asyncAction, CancellationToken cancellationToken = default) 
            => _handler.Enqueue(async () => await asyncAction.Invoke(), cancellationToken);

        protected Task<T> InvokeAsync<T>(Func<Task<T>> asyncFunction, CancellationToken cancellationToken = default) 
            => _handler.Enqueue(async () => await asyncFunction.Invoke(), cancellationToken);
         
        protected virtual void Shutdown()
        {
        }

        public virtual async ValueTask DisposeAsync()
        {
            await _handler.Stop();
        }

    }
}
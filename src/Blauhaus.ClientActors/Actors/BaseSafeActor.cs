using Blauhaus.Common.Utils.Disposables;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blauhaus.ClientActors.Actors
{
    public abstract class BaseActor : BasePublisher, IAsyncDisposable
    {
        protected Task InvokeAsync(Action action, CancellationToken cancellationToken = default) 
            => Task.Run(action.Invoke);

        protected Task<T> InvokeAsync<T>(Func<T> function, CancellationToken cancellationToken = default) 
            => Task.Run(function.Invoke);

        protected Task InvokeAsync(Func<Task> asyncAction, CancellationToken cancellationToken = default) 
            => asyncAction.Invoke();

        protected Task<T> InvokeAsync<T>(Func<Task<T>> asyncFunction, CancellationToken cancellationToken = default)
            => asyncFunction.Invoke();


        public virtual ValueTask DisposeAsync()
        {
            return new ValueTask();
        }
    }
}
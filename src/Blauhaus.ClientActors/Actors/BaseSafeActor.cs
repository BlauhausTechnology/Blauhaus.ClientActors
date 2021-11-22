using Blauhaus.Common.Utils.Disposables;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blauhaus.ClientActors.Actors
{
    public abstract class BaseActor : BasePublisher, IAsyncDisposable
    {
        protected Task InvokeAsync(Action action) 
            => Task.Run(action.Invoke);

        protected Task<T> InvokeAsync<T>(Func<T> function) 
            => Task.Run(function.Invoke);

        protected Task InvokeAsync(Func<Task> asyncAction) 
            => asyncAction.Invoke();

        protected Task<T> InvokeAsync<T>(Func<Task<T>> asyncFunction)
            => asyncFunction.Invoke();


        public virtual ValueTask DisposeAsync()
        {
            return new ValueTask();
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks; 

namespace Blauhaus.ClientActors
{
    public abstract class BaseBlockingActor : IAsyncDisposable
    {
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1);
          
        //todo make this an execution option inside BaseActor instead of a separate class and write some tests!!!!

        protected void Do(Action action)
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
        }
        protected T Do<T>(Func<T> function)
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
        }

        protected async Task DoAsync(Func<Task> asyncAction)
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

        protected async Task<T> DoAsync<T>(Func<Task<T>> asyncFunction) 
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
         

        public virtual ValueTask DisposeAsync()
        {
            _lock.Dispose();
            return new ValueTask();
        }
    }
}
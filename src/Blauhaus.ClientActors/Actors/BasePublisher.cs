using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.Common.Utils.Disposables;

namespace Blauhaus.ClientActors.Actors
{
    public abstract class BasePublisher
    {
        
        private Dictionary<Type, List<Func<object, Task>>>? _subscriptions;

        protected async Task<IDisposable> SubscribeAsync<TModel>(Func<TModel, Task> handler, Func<Task<TModel>> loader)
        {

            _subscriptions ??= new Dictionary<Type, List<Func<object, Task>>>();

            if (!_subscriptions.ContainsKey(typeof(TModel)))
            {
                _subscriptions[typeof(TModel)] = new List<Func<object, Task>>();
            }

            Func<object, Task> subscription = obj => handler.Invoke((TModel) obj);

            _subscriptions[typeof(TModel)].Add(subscription);

            var model = await loader.Invoke();

            if (model != null)
            {
                await subscription.Invoke(model);
            }

            return (IDisposable) new ActionDisposable(() =>
            {
                _subscriptions[typeof(TModel)].Remove(subscription);
            });
        }

        protected Task UpdateSubscribersAsync<TUpdate>(TUpdate update)
        {
            if (_subscriptions != null  && _subscriptions.Count > 0 && update != null)
            {
                var tasks = new List<Task>();
                foreach (var sub in _subscriptions)
                {
                    foreach (var subscription in sub.Value)
                    {
                        tasks.Add(subscription.Invoke(update));
                    }
                } 
                return Task.WhenAll(tasks);
            }

            return Task.CompletedTask;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.Common.Utils.Disposables;

namespace Blauhaus.ClientActors.Actors
{
    public abstract class BasePublisher
    {
        
        private Dictionary<Type, List<Func<object, Task>>>? _subscriptions;

        protected async Task<IDisposable> SubscribeAsync<T>(Func<T, Task> handler, Func<Task<T>>? initialLoader = null)
        {

            _subscriptions ??= new Dictionary<Type, List<Func<object, Task>>>();

            if (!_subscriptions.ContainsKey(typeof(T)))
            {
                _subscriptions[typeof(T)] = new List<Func<object, Task>>();
            }

            Func<object, Task> subscription = obj => handler.Invoke((T) obj);

            _subscriptions[typeof(T)].Add(subscription);

            if (initialLoader != null)
            {
                var initialUpdate = await initialLoader.Invoke();

                if (initialUpdate != null)
                {
                    await subscription.Invoke(initialUpdate);
                }
            }

            return new ActionDisposable(() =>
            {
                _subscriptions[typeof(T)].Remove(subscription);
            });
        }

        protected Task UpdateSubscribersAsync<T>(T update)
        {
            if (_subscriptions != null  && _subscriptions.Count > 0 && update != null)
            {
                var tasks = new List<Task>();
                foreach (var sub in _subscriptions.Where(sub => sub.Key == typeof(T) || sub.Key.IsInstanceOfType(typeof(T))))
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Blauhaus.Common.Utils.Disposables;

namespace Blauhaus.ClientActors.Actors
{
    public abstract class BasePublisher
    {
        
        private Dictionary<string, List<Func<object, Task>>>? _subscriptions;

        protected async Task<IDisposable> SubscribeAsync<T>(Func<T, Task> handler, Func<Task<T>>? initialLoader = null)
        {
            _subscriptions ??= new Dictionary<string, List<Func<object, Task>>>();

            var subscriptionName = GetName<T>();

            if (!_subscriptions.ContainsKey(subscriptionName))
            {
                _subscriptions[subscriptionName] = new List<Func<object, Task>>();
            }

            Func<object, Task> subscription = x => handler.Invoke((T) x);
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));

            _subscriptions[subscriptionName].Add(subscription);

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
                _subscriptions[subscriptionName].Remove(subscription);
            });
        }

        protected Task UpdateSubscribersAsync<T>(T update)
        {
            if (_subscriptions != null  && _subscriptions.Count > 0 && update != null)
            {
                var subscriptionName = GetName<T>();
                var subscriptions = _subscriptions.Where(sub => sub.Key == subscriptionName).Select(x => x.Value);
                 
                var tasks = new List<Task>();
                foreach (var subscription in subscriptions)
                {
                    tasks.AddRange(subscription.Select(handler => handler.Invoke(update)));
                } 
                return Task.WhenAll(tasks);
            }

            return Task.CompletedTask;
        }
         
        private static string GetName<T>()
        {
            return typeof(T).Name.TrimStart('I');
        }
         
    }
}
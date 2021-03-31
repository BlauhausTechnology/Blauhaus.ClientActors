using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.ClientActors.Actors;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.ClientActors.Containers
{
    public class ActorContainer<TActor, TId> : BaseActor, IActorContainer<TActor, TId> 
        where TActor : class, IActor<TId>
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly Dictionary<TId, TActor> _actorCache = new();

        public ActorContainer(
            IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public Task<TActor> GetOneAsync(TId actorId)
        {
            return InvokeInterleavedAsync(async () => await GetActorAsync(actorId)); 
        }

        public Task<IReadOnlyList<TActor>> GetAsync(IEnumerable<TId> actorIds)
        {
            return InvokeInterleavedAsync(async () => await GetActorsAsync(actorIds));
        }

        public Task<TActor> UseOneAsync(TId actorId)
        {
            return InvokeInterleavedAsync(async () =>
            {
                if (_actorCache.TryGetValue(actorId, out var existingActor))
                {
                    return existingActor;
                }

                var newActor = _serviceLocator.Resolve<TActor>();
                await newActor.InitializeAsync(actorId);

                return newActor;
            });
        }

        public Task<IReadOnlyList<TActor>> UseAsync(IEnumerable<TId> actorIds)
        {
            return InvokeInterleavedAsync(async () =>
            {
                var actorsToReturn = new List<TActor>();

                foreach (var actorId in actorIds)
                {
                    if (_actorCache.TryGetValue(actorId, out var existingActor))
                    {
                        actorsToReturn.Add(existingActor);
                    }
                    else
                    {
                        actorsToReturn.Add(await UseActorAsync(actorId));
                    }
                }

                return (IReadOnlyList<TActor>) actorsToReturn;
            });
        }

        public Task<IReadOnlyList<TActor>> GetActiveAsync()
        {
            return InvokeInterleavedAsync(() => (IReadOnlyList<TActor>)_actorCache.Values.ToList());
        }

        public Task<IReadOnlyList<TActor>> GetActiveAsync(IEnumerable<TId> requiredIds)
        {
            return InvokeInterleavedAsync(() => (IReadOnlyList<TActor>)_actorCache
                .Where(x => requiredIds.Contains(x.Key))
                .Select(x => x.Value)
                .ToList());
        }

        public Task<IReadOnlyList<TActor>> GetActiveAsync(Func<TActor, bool> predicate)
        {
            return InvokeInterleavedAsync(() => (IReadOnlyList<TActor>)_actorCache
                .Values.Where(predicate.Invoke).ToList());
        }

        public Task ReloadActiveAsync()
        {
            return InvokeInterleavedAsync(() =>
            { 
                return Task.WhenAll(_actorCache.Values
                    .Select(activeActor => activeActor.ReloadAsync()).ToList());
            });
        }

        public Task ReloadIfActiveAsync(IEnumerable<TId> actorIds)
        {
            return InvokeInterleavedAsync(() =>
            {
                var reloadTasks = new List<Task>();

                foreach (var keyValuePair in _actorCache.Where(x => actorIds.Contains(x.Key)))
                {
                    reloadTasks.Add(keyValuePair.Value.ReloadAsync());
                }

                return Task.WhenAll(reloadTasks);
            });
        }

        public Task RemoveAllAsync()
        {
            return InvokeInterleavedAsync(async () =>
            {
                foreach (var actor in _actorCache)
                {
                    await actor.Value.DisposeAsync();
                }
                _actorCache.Clear();
            });
        }

        public Task RemoveAsync(TId actorId)
        {
            return InvokeInterleavedAsync(async () =>
            {
                if (_actorCache.TryGetValue(actorId, out var actorToReove))
                {
                    await actorToReove.DisposeAsync();
                    _actorCache.Remove(actorId);
                }
            });
        }

        public Task RemoveAsync(IEnumerable<TId> requiredIds)
        {
            return InvokeInterleavedAsync(async () =>
            {
                foreach (var requiredId in requiredIds)
                {
                    if (_actorCache.TryGetValue(requiredId, out var actorToReove))
                    {
                        await actorToReove.DisposeAsync();
                        _actorCache.Remove(requiredId);
                    }
                } 
            });
        }

        public Task RemoveAsync(Func<TActor, bool> predicate)
        {
            return InvokeInterleavedAsync(async () =>
            {
                var actorsToRemove = _actorCache.Where(x => predicate(x.Value)).ToList();
                foreach (var actorToRemove in actorsToRemove)
                {
                    await actorToRemove.Value.DisposeAsync();
                    _actorCache.Remove(actorToRemove.Key);
                }
            });
        }

        
        protected async Task<TActor> GetActorAsync(TId actorId)
        {
            if (_actorCache.TryGetValue(actorId, out var existingActor))
            {
                return existingActor;
            }

            var newActor = _serviceLocator.Resolve<TActor>();
            await newActor.InitializeAsync(actorId);
            _actorCache[actorId] = newActor;

            return newActor;
        }

        protected async Task<IReadOnlyList<TActor>> GetActorsAsync(IEnumerable<TId> ids)
        {
            var getActorTasks = new List<Task<TActor>>();
            foreach (var actorId in ids)
            {
                getActorTasks.Add(GetActorAsync(actorId));
            }

            return await Task.WhenAll(getActorTasks);
        }

        private async Task<TActor> UseActorAsync(TId actorId)
        {
            if (_actorCache.TryGetValue(actorId, out var existingActor))
            {
                return existingActor;
            }

            var newActor = _serviceLocator.Resolve<TActor>();
            await newActor.InitializeAsync(actorId);

            return newActor;
        }

        
    }
}
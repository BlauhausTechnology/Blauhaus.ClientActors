﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.ClientActors
{
    public sealed class ActorContainer<TActor> : BaseActor, IActorContainer<TActor> where TActor : class, IActor
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly Dictionary<string, TActor> _actorCache = new Dictionary<string, TActor>();

        public ActorContainer(
            IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public Task<TActor> GetAsync(string actorId)
        {
            return DoAsync(async () =>
            {
                if (_actorCache.TryGetValue(actorId, out var existingActor))
                {
                    return existingActor;
                }

                var newActor = _serviceLocator.Resolve<TActor>();
                await newActor.InitializeAsync(actorId);
                _actorCache[actorId] = newActor;

                return newActor;
            });
        }

        public Task<IReadOnlyList<TActor>> GetAsync(IEnumerable<string> actorIds)
        {
            return DoAsync(async () =>
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
                        actorsToReturn.Add(await GetAsync(actorId));
                    }
                }

                return (IReadOnlyList<TActor>) actorsToReturn;
            });
        }

        public Task<TActor> UseAsync(string actorId)
        {
            return DoAsync(async () =>
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

        public Task<IReadOnlyList<TActor>> UseAsync(IEnumerable<string> actorIds)
        {
            return DoAsync(async () =>
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
                        actorsToReturn.Add(await UseAsync(actorId));
                    }
                }

                return (IReadOnlyList<TActor>) actorsToReturn;
            });
        }

        public Task<IReadOnlyList<TActor>> GetActiveAsync()
        {
            return DoAsync(() => Task.FromResult<IReadOnlyList<TActor>>(_actorCache.Values.ToList()));
        }

        public Task<IReadOnlyList<TActor>> GetActiveAsync(IEnumerable<string> requiredIds)
        {
            return DoAsync(() => Task.FromResult<IReadOnlyList<TActor>>(_actorCache
                .Where(x => requiredIds.Contains(x.Key))
                .Select(x => x.Value)
                .ToList()));
        }

        public Task<IReadOnlyList<TActor>> GetActiveAsync(Func<TActor, bool> predicate)
        {
            return DoAsync(() => Task.FromResult<IReadOnlyList<TActor>>(_actorCache.Values
                .Where(predicate.Invoke) 
                .ToList()));
        }

        public Task RemoveAllAsync()
        {
            return DoAsync(async () =>
            {
                foreach (var actor in _actorCache)
                {
                    await actor.Value.DisposeAsync();
                }
                _actorCache.Clear();
            });
        }

        public Task RemoveAsync(IEnumerable<string> requiredIds)
        {
            return DoAsync(async () =>
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
            return DoAsync(async () =>
            {
                var actorsToRemove = _actorCache.Where(x => predicate(x.Value)).ToList();
                foreach (var actorToRemove in actorsToRemove)
                {
                    await actorToRemove.Value.DisposeAsync();
                    _actorCache.Remove(actorToRemove.Key);
                }
            });
        }
    }
}
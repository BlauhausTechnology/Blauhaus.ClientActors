using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.ClientActors.StandaloneActors;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.ClientActors.Manager
{
    public sealed class ActorContainer<TActor> : BaseActor, IActorContainer<TActor> where TActor : class, IInitializeById
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
    }
}
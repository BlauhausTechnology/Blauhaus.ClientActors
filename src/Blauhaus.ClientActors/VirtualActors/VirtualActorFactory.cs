using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.ClientActors.VirtualActors
{
    public class VirtualActorFactory : IVirtualActorFactory
    {
        private readonly IServiceLocator _serviceLocator;

        private readonly ConcurrentDictionary<Type, object> _actorsWithoutId;
        private readonly ConcurrentDictionary<Type, Dictionary<string, object>> _actorsWithId = new ConcurrentDictionary<Type, Dictionary<string, object>>();

        public VirtualActorFactory(IServiceLocator serviceLocator)
        {
            _actorsWithoutId = new ConcurrentDictionary<Type, object>();
            _serviceLocator = serviceLocator;
        }

        public IVirtualActor<TActor> GetById<TActor>(string actorId) where TActor : class, IInitializeById
        { 
            var virtualActor = GetAndInitializeActorWithId<TActor>(actorId);

            if (_actorsWithId.TryGetValue(typeof(TActor), out var actorCache))
            {
                actorCache[actorId] = virtualActor;
            }
            else
            {
                _actorsWithId[typeof(TActor)] = new Dictionary<string, object> {[actorId] = virtualActor};
            }
            
            return virtualActor;
        }

        public IVirtualActor<TActor> UseById<TActor>(string actorId) where TActor : class, IInitializeById
        {
            return GetAndInitializeActorWithId<TActor>(actorId);
        }

        public IVirtualActor<TActor> Get<TActor>() where TActor : class, IInitialize
        {
            var virtualActor = GetAndInitializeActorWithoutId<TActor>();
            _actorsWithoutId[typeof(TActor)] = virtualActor;
            return virtualActor;
        }

        public IVirtualActor<TActor> Use<TActor>() where TActor : class, IInitialize
        {
            return GetAndInitializeActorWithoutId<TActor>();
        }

        public IReadOnlyList<IVirtualActor<TActor>> GetActive<TActor>()
        { 
            if (_actorsWithId.TryGetValue(typeof(TActor), out var activeActors))
            {
                return activeActors.Select(x => (VirtualActor<TActor>) x.Value).ToList();
            }
            return new List<IVirtualActor<TActor>>();
        }

        private VirtualActor<TActor> GetAndInitializeActorWithId<TActor>(string actorId) where TActor : class, IInitializeById
        {
            if (_actorsWithId.TryGetValue(typeof(TActor), out var virtualActorCache))
            {
                if (virtualActorCache.TryGetValue(actorId, out var actorObject))
                {
                    return (VirtualActor<TActor>) actorObject;
                }
            }
            
            var actor = _serviceLocator.Resolve<TActor>();
            var virtualActor = new VirtualActor<TActor>(actor);
            
            //don't need to await this, it will be the first item in the actor's processing queue
            Task.Run(async () => await virtualActor.InvokeAsync(x => x.InitializeAsync, actorId, CancellationToken.None));

            return virtualActor;
        }

        private VirtualActor<TActor> GetAndInitializeActorWithoutId<TActor>() where TActor : class, IInitialize
        { 
            if (_actorsWithoutId.TryGetValue(typeof(TActor), out var actorObject))
            {
                return (VirtualActor<TActor>) actorObject;
            }
            
            var actor = _serviceLocator.Resolve<TActor>();
            var virtualActor = new VirtualActor<TActor>(actor);

            Task.Run(async () => await virtualActor.InvokeAsync(x => x.InitializeAsync, CancellationToken.None));
            
            return virtualActor;
        }
    }
}
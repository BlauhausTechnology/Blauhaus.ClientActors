using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.ClientActors.VirtualActors
{
    public class VirtualActorFactory : IVirtualActorFactory
    {
        private readonly IServiceLocator _serviceLocator;

        private readonly ConcurrentDictionary<string, object> _actors;

        public VirtualActorFactory(IServiceLocator serviceLocator)
        {
            _actors = new ConcurrentDictionary<string, object>();
            _serviceLocator = serviceLocator;
        }

        public IVirtualActor<TActor> Get<TActor>(string actorId) where TActor : class, IInitializeById
        {
            var virtualActor = GetAndInitializeVirtualActor<TActor>(actorId);
            _actors[$"{typeof(TActor).FullName}|{actorId}"] = virtualActor;
            return virtualActor;
        }

        public IVirtualActor<TActor> GetTransient<TActor>(string actorId) where TActor : class, IInitializeById
        {
            return GetAndInitializeVirtualActor<TActor>(actorId);
        }

        public IVirtualActor<TActor> Get<TActor>() where TActor : class
        {
            var virtualActor = GetVirtualActor<TActor>();
            _actors[$"{typeof(TActor).FullName}"] = virtualActor;
            return virtualActor;
        }

        public IVirtualActor<TActor> GetTransient<TActor>() where TActor : class
        {
            return GetVirtualActor<TActor>();
        }

        private VirtualActor<TActor> GetAndInitializeVirtualActor<TActor>(string actorId) where TActor : class, IInitializeById
        {
            var actorKey = $"{typeof(TActor).FullName}|{actorId}";
            if (_actors.TryGetValue(actorKey, out var actorObject))
            {
                return (VirtualActor<TActor>) actorObject;
            }
            
            var actor = _serviceLocator.Resolve<TActor>();
            var virtualActor = new VirtualActor<TActor>(actor);
            
            //don't need to await this, it will be the first item in the actor's processing queue
            Task.Run(async () => await virtualActor.InvokeAsync(x => x.InitializeAsync, actorId, CancellationToken.None));

            return virtualActor;
        }

        private VirtualActor<TActor> GetVirtualActor<TActor>() where TActor : class
        {
            var actorKey = $"{typeof(TActor).FullName}";
            if (_actors.TryGetValue(actorKey, out var actorObject))
            {
                return (VirtualActor<TActor>) actorObject;
            }
            
            var actor = _serviceLocator.Resolve<TActor>();
            var virtualActor = new VirtualActor<TActor>(actor);
            
            return virtualActor;
        }
    }
}
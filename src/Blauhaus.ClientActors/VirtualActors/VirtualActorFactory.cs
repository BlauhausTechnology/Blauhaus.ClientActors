using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.ClientActors.VirtualActors
{
    public class VirtualActorFactory : IVirtualActorFactory
    {
        private readonly IServiceLocator _serviceLocator;

        public VirtualActorFactory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public IVirtualActor<TActor> Get<TActor>(string actorId) where TActor : class, IClientActor
        {
            return new VirtualActor<TActor>(_serviceLocator, actorId);
        }
    }
}
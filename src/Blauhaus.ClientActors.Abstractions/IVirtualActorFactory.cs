namespace Blauhaus.ClientActors.Abstractions
{
    public interface IVirtualActorFactory
    {
        IVirtualActor<TActor> Get<TActor>(string actorId) where TActor : class, IInitializeById;
        IVirtualActor<TActor> GetTransient<TActor>(string actorId) where TActor : class, IInitializeById;

        IVirtualActor<TActor> Get<TActor>() where TActor : class;
        IVirtualActor<TActor> GetTransient<TActor>() where TActor : class;
    }

}
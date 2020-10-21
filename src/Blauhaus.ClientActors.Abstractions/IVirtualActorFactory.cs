namespace Blauhaus.ClientActors.Abstractions
{
    public interface IVirtualActorFactory
    {
        IVirtualActor<TVirtualActor> Get<TVirtualActor>(string actorId) where TVirtualActor : class, IClientActor;
    }
}
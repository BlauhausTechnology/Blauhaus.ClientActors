using System.Collections.Generic;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IActorFactory
    {
        TActor Get<TActor>(string actorId) where TActor : class, IInitializeById;
        TActor Use<TActor>(string actorId) where TActor : class, IInitializeById;
        IReadOnlyList<TActor> GetActive<TActor>() where TActor : class, IInitializeById;
    }
}
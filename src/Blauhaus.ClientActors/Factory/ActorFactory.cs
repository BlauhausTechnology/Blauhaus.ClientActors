using System.Collections.Generic;
using Blauhaus.ClientActors.Abstractions;

namespace Blauhaus.ClientActors.Factory
{
    public class ActorFactory : IActorFactory
    {
        public TActor Get<TActor>(string actorId) where TActor : class, IInitializeById
        {
            throw new System.NotImplementedException();
        }

        public TActor Use<TActor>(string actorId) where TActor : class, IInitializeById
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyList<TActor> GetActive<TActor>() where TActor : class, IInitializeById
        {
            throw new System.NotImplementedException();
        }
    }
}
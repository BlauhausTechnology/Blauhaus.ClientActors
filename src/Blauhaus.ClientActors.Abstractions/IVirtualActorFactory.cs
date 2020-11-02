using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IVirtualActorFactory
    {
        IVirtualActor<TActor> GetById<TActor>(string actorId) where TActor : class, IInitializeById;
        IVirtualActor<TActor> UseById<TActor>(string actorId) where TActor : class, IInitializeById;

        IVirtualActor<TActor> Get<TActor>() where TActor : class, IInitialize;
        IVirtualActor<TActor> Use<TActor>() where TActor : class, IInitialize;

        IReadOnlyList<IVirtualActor<TActor>> GetActive<TActor>();
    }

}
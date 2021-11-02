using Blauhaus.Common.Abstractions;
using System;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IDtoModelActor<TModel, in TId> : IModelActor<TModel>, IAsyncInitializable<TId>, IDisposable
    {
        
    }
}
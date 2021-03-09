using System;
using Blauhaus.Common.Abstractions;

namespace Blauhaus.ClientActors.Abstractions
{

    public interface IActor<TId> : IAsyncDisposable, IAsyncInitializable<TId>, IAsyncReloadable, IHasId<TId>
    {
        
    }
}
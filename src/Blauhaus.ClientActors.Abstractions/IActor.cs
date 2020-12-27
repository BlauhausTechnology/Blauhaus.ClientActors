using System;
using Blauhaus.Common.Utils.Contracts;

namespace Blauhaus.ClientActors.Abstractions
{

    public interface IActor<TId> : IAsyncDisposable, IAsyncInitializable<TId>, IAsyncReloadable, IHasId<TId>
    {
        
    }
}
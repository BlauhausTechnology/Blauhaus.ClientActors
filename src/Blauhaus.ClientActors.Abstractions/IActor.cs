using System;
using Blauhaus.Common.Utils.Contracts;

namespace Blauhaus.ClientActors.Abstractions
{

    public interface IActor<in TId> : IAsyncDisposable, IAsyncInitializable<TId>, IAsyncReloadable
    {
        
    }
}
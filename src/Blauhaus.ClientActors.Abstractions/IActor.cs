using System;
using Blauhaus.Common.Utils.Contracts;

namespace Blauhaus.ClientActors.Abstractions
{

    public interface IActor<in TId> : IAsyncDisposable, IInitialize<TId>, IReloadable
    {
        
    }
}
using System;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IActor : IAsyncDisposable, IInitializeById, IReloadable
    {
        
    }
}
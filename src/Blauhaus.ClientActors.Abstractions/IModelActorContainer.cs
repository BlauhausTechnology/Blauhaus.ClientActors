using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Blauhaus.Common.Utils.Contracts;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IModelActorContainer<TActor, in TId, TModel> : IActorContainer<TActor, TId> 
        where TActor : class, IModelActor<TId, TModel> 
        where TModel : IId<TId>
    {
        Task<TModel> GetModelAsync(TId id);
        Task<IReadOnlyList<TModel>> GetModelsAsync(IEnumerable<TId> actorIds);
        Task<IDisposable> SubscribeAsync(TId id, Func<TModel, Task> handler);
    }
}
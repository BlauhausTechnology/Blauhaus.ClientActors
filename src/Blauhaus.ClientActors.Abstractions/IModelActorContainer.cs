using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IModelActorContainer<TActor, in TId, TModel> : IActorContainer<TActor, TId>
        where TActor : class, IModelActor<TId, TModel> 
        where TModel : IHasId<TId>
    {
        Task<TModel> GetModelAsync(TId id);
        Task<IReadOnlyList<TModel>> GetModelsAsync(IEnumerable<TId> actorIds);
        Task<IReadOnlyList<TModel>> GetActiveModelsAsync();

        Task<IDisposable> SubscribeToModelAsync(TId id, Func<TModel, Task> handler);
        Task<IDisposable> SubscribeToActiveModelsAsync(Func<TModel, Task> handler);
    }
}
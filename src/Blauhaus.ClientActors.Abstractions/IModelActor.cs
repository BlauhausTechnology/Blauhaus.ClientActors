using System;
using System.Threading.Tasks;
using Blauhaus.Common.Utils.Contracts;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IModelActor<in TId, TModel> : IActor<TId> , IAsyncPublisher<TModel>
        where TModel : IHasId<TId>
    {

        Task<TModel> GetModelAsync();
    }
}
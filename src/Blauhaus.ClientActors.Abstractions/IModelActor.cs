using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IModelActor<TId, TModel> : IActor<TId> , IAsyncPublisher<TModel>
        where TModel : IHasId<TId>
    {

        Task<TModel> GetModelAsync();
    }
}
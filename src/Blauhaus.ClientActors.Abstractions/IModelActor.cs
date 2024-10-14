using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;

namespace Blauhaus.ClientActors.Abstractions;

public interface IModelActor<TModel, TId> : IActor<TId>, IAsyncPublisher<TModel>
    where TModel : IHasId<TId>
{
    Task<TModel> GetModelAsync();
}
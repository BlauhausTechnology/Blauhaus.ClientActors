using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;

namespace Blauhaus.ClientActors.Abstractions
{ 
    public interface IModelActor<TModel> : IAsyncPublisher<TModel>, IAsyncReloadable
    {
        Task<TModel> GetModelAsync();
    }
    
    public interface IModelActor<TId, TModel> : IActor<TId>, IModelActor<TModel>
        where TModel : IHasId<TId>
    {
    }
}
using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;
using Blauhaus.Domain.Abstractions.DtoHandlers;

namespace Blauhaus.ClientActors.Abstractions
{ 
    public interface IModelActor<TModel> : IAsyncPublisher<TModel>, IAsyncReloadable, IModelOwner<TModel>
    {
    }
    
    public interface IModelActor<TId, TModel> : IActor<TId>, IModelActor<TModel>
        where TModel : IHasId<TId>
    {
    }
}
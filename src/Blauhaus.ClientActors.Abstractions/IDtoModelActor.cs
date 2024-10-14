using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;

namespace Blauhaus.ClientActors.Abstractions;

public interface IDtoModelActor<TModel, TDto, TId> : IModelActor<TModel, TId> 
    where TModel : IHasId<TId>
{
    Task<TDto> GetDtoAsync();
}
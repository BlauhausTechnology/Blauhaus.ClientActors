using System;
using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;

namespace Blauhaus.ClientActors.Abstractions;

public interface IDtoCache<TDto, in TId> : IDtoLoader<TDto, TId> 
    where TDto : class, IHasId<TId>
    where TId : IEquatable<TId>
{
    Task HandleAsync(TDto dto);
    Task DeleteOneAsync(TId id);
    Task DeleteAllAsync();
}
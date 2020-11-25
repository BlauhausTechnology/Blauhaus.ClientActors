using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blauhaus.ClientActors.Abstractions
{
     
    public interface IActorContainer<TActor, in TId> where TActor : class, IActor<TId>
    { 
        Task<TActor> GetAsync(TId actorId);
        Task<IReadOnlyList<TActor>> GetAsync(IEnumerable<TId> actorIds);

         
        Task<TActor> UseAsync(TId actorId);
        Task<IReadOnlyList<TActor>> UseAsync(IEnumerable<TId> actorIds);

         
        Task<IReadOnlyList<TActor>> GetActiveAsync();
         
        Task<IReadOnlyList<TActor>> GetActiveAsync(IEnumerable<TId> actorIds);
         
        Task<IReadOnlyList<TActor>> GetActiveAsync(Func<TActor, bool> predicate);

        Task ReloadActiveAsync();
        Task ReloadIfActiveAsync(IEnumerable<TId> actorIds);

        Task RemoveAllAsync();
        Task RemoveAsync(IEnumerable<TId> actorIds);
        Task RemoveAsync(Func<TActor, bool> predicate);
    }

}
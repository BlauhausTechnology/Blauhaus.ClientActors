using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IActorContainer<TActor> where TActor : class, IActor
    { 
        Task<TActor> GetAsync(string actorId);
        Task<IReadOnlyList<TActor>> GetAsync(IEnumerable<string> actorIds);

         
        Task<TActor> UseAsync(string actorId);
        Task<IReadOnlyList<TActor>> UseAsync(IEnumerable<string> actorIds);

         
        Task<IReadOnlyList<TActor>> GetActiveAsync();
         
        Task<IReadOnlyList<TActor>> GetActiveAsync(IEnumerable<string> actorIds);
         
        Task<IReadOnlyList<TActor>> GetActiveAsync(Func<TActor, bool> predicate);

        Task RemoveAllAsync();
        Task RemoveAsync(IEnumerable<string> actorIds);
        Task RemoveAsync(Func<TActor, bool> predicate);
    }

}
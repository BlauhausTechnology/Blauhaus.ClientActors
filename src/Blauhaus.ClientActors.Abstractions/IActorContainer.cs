using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IActorContainer<TActor> where TActor : class, IInitializeById
    {
        /// <summary>
        /// Returns existing actor if there is one. If there isn't, creates and initializes the actor and retains it in the cache 
        /// </summary>
        Task<TActor> GetAsync(string actorId);

        /// <summary>
        /// Returns existing actor if there is one. If there isn't, creates and initializes the actor but does not cache it 
        /// </summary>
        Task<TActor> UseAsync(string actorId);

        /// <summary>
        /// Retruns all actors currently in the cache
        /// </summary>
        Task<IReadOnlyList<TActor>> GetActiveAsync();

        /// <summary>
        /// Retruns actors currently in the cache with matching ids
        /// </summary>
        Task<IReadOnlyList<TActor>> GetActiveAsync(IEnumerable<string> requiredIds);

        /// <summary>
        /// Retruns actors currently in the cache matching the given condition
        /// </summary>
        Task<IReadOnlyList<TActor>> GetActiveAsync(Func<TActor, bool> predicate);
    }

}
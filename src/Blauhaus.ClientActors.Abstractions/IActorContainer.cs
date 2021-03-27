using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blauhaus.ClientActors.Abstractions
{
     
    public interface IActorContainer<TActor, in TId> 
        where TActor : class, IActor<TId>
    { 
        
        /// <summary>
        /// Returns actor with matching id if it exists, otherwise creates a new one and caches the result
        /// </summary>
        Task<TActor> GetOneAsync(TId actorId); 
        /// <summary>
        /// Returns actors matching all given ids. Cached actors are returned, new actors are created and returned and added to the cache 
        /// </summary>
        Task<IReadOnlyList<TActor>> GetAsync(IEnumerable<TId> actorIds);

         
        /// <summary>
        /// Returns actor with matching id if it exists, otherwise creates a new one but does NOT cache the result
        /// </summary>
        Task<TActor> UseOneAsync(TId actorId);
        /// <summary>
        /// Returns actors matching all given ids. Cached actors are returned, new actors are created and returned but NOT added to the cache. 
        /// </summary>
        Task<IReadOnlyList<TActor>> UseAsync(IEnumerable<TId> actorIds);

        /// <summary>
        /// Returns all actors that have been created and added to the cache
        /// </summary>
        Task<IReadOnlyList<TActor>> GetActiveAsync();
        /// <summary>
        /// Returns any existing actors from the cache that match the given ids. No new actors are created if none exist for any of the ids.
        /// </summary>
        Task<IReadOnlyList<TActor>> GetActiveAsync(IEnumerable<TId> actorIds);
        /// <summary>
        /// Returns any existing actors from the cache that match the given predicate. No new actors are created if none exist for the predicate.
        /// </summary>
        Task<IReadOnlyList<TActor>> GetActiveAsync(Func<TActor, bool> predicate);

        /// <summary>
        /// Invokes ReloadAsync on all actors in the cache
        /// </summary>
        Task ReloadActiveAsync();
        /// <summary>
        /// Invokes ReloadAsync on all actors in the cache matching the given ids. If any ids are not found in the cache, they are ignored. No new actors are created.
        /// </summary>
        Task ReloadIfActiveAsync(IEnumerable<TId> actorIds);

        /// <summary>
        /// Clears all actors from the cache
        /// </summary>
        Task RemoveAllAsync();
        /// <summary>
        /// Removes from the cache only the actors with the given ids. 
        /// </summary>
        Task RemoveAsync(IEnumerable<TId> actorIds);
        /// <summary>
        /// Removes from the cache only the actors that match the predicate 
        /// </summary>
        Task RemoveAsync(Func<TActor, bool> predicate);
    }

}
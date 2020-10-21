using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using IActor = Blauhaus.ClientActors.Abstractions.IActor;

[assembly:InternalsVisibleTo("Blauhaus.ClientActors.Tests")]
[assembly:InternalsVisibleTo("Blauhaus.ClientActors.TestHelpers")]
namespace Blauhaus.ClientActors.VirtualActors
{
    internal class VirtualActorCache 
    {
        private readonly ConcurrentDictionary<string, IActor> _actors = new ConcurrentDictionary<string, IActor>();
         

        public TActor? Get<TActor>(string id) where TActor : class, IActor
        {
            var actorKey = $"{typeof(TActor)}|{id}";
            return _actors.TryGetValue(actorKey, out var actor)
                ? (TActor) actor
                : null;
        }

        public void Add<TActor>(string id, TActor actor) where TActor : class, IActor
        {
            var actorKey = $"{typeof(TActor)}|{id}";
            _actors[actorKey] = actor;
        }

        public bool TryRemove<TActor>(string id, out IActor actorToShutdown) where TActor : class, IActor
        {
            return _actors.TryRemove($"{typeof(TActor)}|{id}", out actorToShutdown);
        } 
    }
}
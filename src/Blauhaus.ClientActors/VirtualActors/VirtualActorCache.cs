using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Blauhaus.ClientActors.Abstractions;

[assembly:InternalsVisibleTo("Blauhaus.ClientActors.Tests")]
[assembly:InternalsVisibleTo("Blauhaus.ClientActors.TestHelpers")]
namespace Blauhaus.ClientActors.VirtualActors
{
    //todo this needs to move to the VirtualActorFactory. The Virtual part also needs to be one per actor else the queues get duplicated
    internal class VirtualActorCache 
    {
        private readonly ConcurrentDictionary<string, IInitializeById> _actors = new ConcurrentDictionary<string, IInitializeById>();
         

        public TActor? Get<TActor>(string id) where TActor : class, IInitializeById
        {
            var actorKey = $"{typeof(TActor)}|{id}";
            return _actors.TryGetValue(actorKey, out var actor)
                ? (TActor) actor
                : null;
        }

        public void Add<TActor>(string id, TActor actor) where TActor : class, IInitializeById
        {
            var actorKey = $"{typeof(TActor)}|{id}";
            _actors[actorKey] = actor;
        }

        public bool TryRemove<TActor>(string id, out IInitializeById actorToShutdown) where TActor : class, IInitializeById
        {
            return _actors.TryRemove($"{typeof(TActor)}|{id}", out actorToShutdown);
        } 
    }
}
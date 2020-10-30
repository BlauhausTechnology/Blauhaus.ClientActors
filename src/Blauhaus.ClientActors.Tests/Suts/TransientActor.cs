using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public class TransientActor : IInitializeById
    {
        public Task InitializeAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task ShutdownAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
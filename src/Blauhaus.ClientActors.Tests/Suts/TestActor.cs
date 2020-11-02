using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.ClientActors.StandaloneActors;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public class TestActor : BaseActor, IInitializeById
    {
        public Task InitializeAsync(string id)
        {
            return Task.CompletedTask;
        }
    }
}
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;

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
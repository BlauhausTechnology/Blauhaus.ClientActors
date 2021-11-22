using System.Threading.Tasks;
using Blauhaus.ClientActors.Actors;
using Blauhaus.Common.Abstractions;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public class TestActor : BaseWildActor, IAsyncInitializable<string>
    {
        public Task InitializeAsync(string id)
        {
            return Task.CompletedTask;
        }
    }
}
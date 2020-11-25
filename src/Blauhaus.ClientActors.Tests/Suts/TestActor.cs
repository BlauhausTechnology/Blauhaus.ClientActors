using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.ClientActors.Actors;
using Blauhaus.Common.Utils.Contracts;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public class TestActor : BaseActor, IInitialize<string>
    {
        public Task InitializeAsync(string id)
        {
            return Task.CompletedTask;
        }
    }
}
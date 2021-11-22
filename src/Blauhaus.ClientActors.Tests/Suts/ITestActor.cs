using Blauhaus.Domain.Abstractions.Actors;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public interface ITestActor : IActor<string>
    {
        public string ExtraProperty { get; }
    }
}
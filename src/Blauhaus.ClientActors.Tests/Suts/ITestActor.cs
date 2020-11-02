using Blauhaus.ClientActors.Abstractions;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public interface ITestActor : IActor
    {
        public string ExtraProperty { get; }
    }
}
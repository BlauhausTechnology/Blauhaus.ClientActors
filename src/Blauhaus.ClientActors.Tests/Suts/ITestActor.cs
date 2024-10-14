using Blauhaus.ClientActors.Abstractions;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public interface ITestActor : IActor<string>
    {
        public string ExtraProperty { get; }
    }
}
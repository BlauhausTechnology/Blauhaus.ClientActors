using Blauhaus.ClientActors.Abstractions;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public interface ITestActor : IInitializeById
    {
        public string ExtraProperty { get; }
    }
}
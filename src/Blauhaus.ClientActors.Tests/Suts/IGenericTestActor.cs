using Blauhaus.ClientActors.Abstractions;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public interface IGenericTestActor<T> : IInitializeById, IInitialize
    {
        
    }
}
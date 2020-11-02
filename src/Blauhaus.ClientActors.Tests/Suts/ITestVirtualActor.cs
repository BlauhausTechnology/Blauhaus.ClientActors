using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Domain.Abstractions.CommandHandlers;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public interface ITestVirtualActor : IInitializeById,
        IVoidCommandHandler<MyVoidMessage>,
        ICommandHandler<MyTestResult, MyTestMessage>,
        IInitialize
    {
        
    }
}
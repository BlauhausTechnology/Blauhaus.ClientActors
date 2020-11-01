using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Domain.Abstractions.CommandHandlers;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public interface ITestActor : IInitializeById,
        IVoidCommandHandler<MyVoidMessage>,
        ICommandHandler<MyTestResult, MyTestMessage>,
        IInitialize
    {
        
    }
}
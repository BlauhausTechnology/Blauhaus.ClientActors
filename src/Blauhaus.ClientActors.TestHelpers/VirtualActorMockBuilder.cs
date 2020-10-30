using Blauhaus.ClientActors.Abstractions;
using Blauhaus.TestHelpers.MockBuilders;

namespace Blauhaus.ClientActors.TestHelpers
{
    public class VirtualActorMockBuilder<TClientActor> : BaseMockBuilder<VirtualActorMockBuilder<TClientActor>, IVirtualActor<TClientActor>> where TClientActor : IInitializeById
    {
       
    }
}
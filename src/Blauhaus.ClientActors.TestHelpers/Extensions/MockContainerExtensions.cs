using System;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.TestHelpers;

namespace Blauhaus.ClientActors.TestHelpers.Extensions
{
    public static class MockContainerExtensions
    {
        public static Func<ActorContainerMockBuilder<TActor>> AddMockActorContainer<TActor>(this MockContainer mocks) where TActor : class, IActor
        {
            return mocks.AddMock<ActorContainerMockBuilder<TActor>, IActorContainer<TActor>>();
        }
    }
}
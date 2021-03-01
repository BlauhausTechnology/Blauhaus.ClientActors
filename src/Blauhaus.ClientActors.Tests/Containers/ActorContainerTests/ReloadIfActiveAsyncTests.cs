using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests.Containers.ActorContainerTests.Base;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.Ioc.TestHelpers;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.Containers.ActorContainerTests
{
    public class ReloadIfActiveAsyncTests : BaseActorContainerTest 
    {
        

        [Test]
        public async Task WHEN_call_reload_on_active_actors_with_matching_ids()
        {
            //Arrange
            var mockActiveActor1 = new MockBuilder<ITestActor>();
            var mockActiveActor2 = new MockBuilder<ITestActor>();
            var mockActiveActor3 = new MockBuilder<ITestActor>();
            var mockServiceLocator = new ServiceLocatorMockBuilder().Where_Resolve_returns_sequence(
                new List<ITestActor>
                {
                    mockActiveActor1.Object,
                    mockActiveActor2.Object,
                    mockActiveActor3.Object,
                });
            AddService(mockServiceLocator.Object);
            await Sut.GetOneAsync("1"); 
            await Sut.GetOneAsync("2"); 

            //Act
            await Sut.ReloadIfActiveAsync(new List<string>{"1", "2", "3"});

            //Assert
            mockActiveActor1.Mock.Verify(x => x.ReloadAsync(), Times.Once);
            mockActiveActor2.Mock.Verify(x => x.ReloadAsync(), Times.Once);
            mockActiveActor3.Mock.Verify(x => x.ReloadAsync(), Times.Never);
        }
    }
}
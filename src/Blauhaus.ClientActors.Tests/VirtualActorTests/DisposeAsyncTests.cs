using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.ClientActors.Tests.VirtualActorTests._Base;
using Blauhaus.ClientActors.VirtualActors;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.VirtualActorTests
{
    public interface ITestActor : IClientActor
    {
        public List<int> GetNumbers();
    }

    public class DisposeAsyncTests : BaseVirtualActorTest<ITestActor>
    {
        private MockBuilder<ITestActor> _mockActor;

        public override void Setup()
        {
            base.Setup();

            _mockActor = new MockBuilder<ITestActor>();
            AddService(_mockActor.Object);
        }

        [Test]
        public async Task SHOULD_shutdown_underlying_actor_and_remove_from_cache()
        {
            //Arrange
            await Sut.InvokeAsync<List<int>>(x => x.GetNumbers);

            //Act
            await Sut.DisposeAsync();

            //Assert
            var cachedActor = ServiceLocator.Resolve<VirtualActorCache>().Get<ITestActor>(ActorId);
            Assert.That(cachedActor, Is.Null);
            _mockActor.Mock.Verify(x => x.ShutdownAsync(), Times.Once);

            var newActorRef = ConstructSut();
            await newActorRef.InvokeAsync<List<int>>(x => x.GetNumbers);
            _mockActor.Mock.Verify(x => x.InitializeAsync(It.IsAny<string>()), Times.Exactly(2));  
        }
         

    }
}
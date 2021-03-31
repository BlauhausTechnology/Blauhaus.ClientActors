using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Containers;
using Blauhaus.ClientActors.Tests.Base;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.Containers.ModelActorContainerTests
{
    public class SubscribeAsyncTests : BaseActorTest<ModelActorContainer<ITestModelActor, Guid, ITestModel>>
    {
        private Guid _id;
        private MockBuilder<IDisposable> _mockTestSubscription;

        protected MockBuilder<ITestModelActor> MockTestActor => AddMock<ITestModelActor>().Invoke();

        public override void Setup()
        {
            base.Setup();

            _id = Guid.NewGuid();

            _mockTestSubscription = new MockBuilder<IDisposable>();
            MockTestActor.Mock.Setup(x => x.SubscribeAsync(It.IsAny<Func<ITestModel, Task>>(), null))
                .ReturnsAsync(_mockTestSubscription.Object);

            AddService(MockTestActor.Object);
        }

        
        [Test]
        public async Task WHEN_Actor_does_not_yet_exist_SHOULD_create_and_subscribe()
        {
            //Arrange
            Func<ITestModel, Task> handler = model => Task.CompletedTask;

            //Act
            var result = await Sut.SubscribeToModelAsync(_id, handler);

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync(_id));
            MockTestActor.Mock.Verify(x => x.SubscribeAsync(handler, null));
            Assert.That(result, Is.EqualTo(_mockTestSubscription.Object));
        }

        [Test]
        public async Task WHEN_Actor_does_exist_SHOULD_subscribe_to_it()
        {
            //Arrange
            Func<ITestModel, Task> handler = model => Task.CompletedTask;
            await Sut.SubscribeToModelAsync(_id, handler);

            //Act
            var result = await Sut.SubscribeToModelAsync(_id, handler);

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync(_id), Times.Once);
            MockTestActor.Mock.Verify(x => x.SubscribeAsync(handler, null), Times.Exactly(2));
            Assert.That(result, Is.EqualTo(_mockTestSubscription.Object));
        }
    }
}
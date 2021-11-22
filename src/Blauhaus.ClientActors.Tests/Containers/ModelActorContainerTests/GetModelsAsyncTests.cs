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
    public class GetModelsAsyncTests : BaseActorTest<ModelActorContainer<ITestModelActor, Guid, ITestModel>>
    {
        private MockBuilder<ITestModel> _mockTestModel = null!;
        private Guid _id;

        protected MockBuilder<ITestModelActor> MockTestActor => AddMock<ITestModelActor>().Invoke();

        public override void Setup()
        {
            base.Setup();

            _id = Guid.NewGuid();

            _mockTestModel = new MockBuilder<ITestModel>();

            MockTestActor.Mock.Setup(x => x.GetModelAsync())
                .ReturnsAsync(_mockTestModel.Object);

            AddService(MockTestActor.Object);
        }

        [Test]
        public async Task WHEN_Actor_does_not_yet_exist_SHOULD_create_and_initialize_one()
        {
            //Act
            var result = await Sut.GetModelsAsync(new []{_id });

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync(_id));
            Assert.That(result[0], Is.EqualTo(_mockTestModel.Object));
        }

        [Test]
        public async Task WHEN_Actor_does_exist_SHOULD_return_it()
        {
            //Arrange
            await Sut.GetModelAsync(_id);

            //Act
            var result = await Sut.GetModelsAsync(new [] {_id});

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync(_id), Times.Once);
            Assert.That(result[0], Is.EqualTo(_mockTestModel.Object));
        }

    }
}
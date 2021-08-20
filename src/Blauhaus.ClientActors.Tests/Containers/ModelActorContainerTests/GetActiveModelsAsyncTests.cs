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
    public class GetActiveModelsAsyncTests : BaseActorTest<ModelActorContainer<ITestModelActor, Guid, ITestModel>>
    {
        private MockBuilder<ITestModel> _mockTestModel;
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
        public async Task SHOULD_return_all_active()
        {
            //Arrange
            await Sut.GetModelAsync(_id);

            //Act
            var result = await Sut.GetActiveModelsAsync();

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync(_id), Times.Once);
            Assert.That(result[0], Is.EqualTo(_mockTestModel.Object));
            Assert.That(result.Count, Is.EqualTo(1));
        }
    }
}
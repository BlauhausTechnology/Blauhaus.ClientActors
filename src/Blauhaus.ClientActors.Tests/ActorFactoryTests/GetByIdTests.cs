using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests._Base;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.ClientActors.VirtualActors;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.ActorFactoryTests
{
    public class GetByIdTests : BaseActorTest<VirtualActorFactory>
    {

        private MockBuilder<ITestActor> MockTestActor => AddMock<ITestActor>().Invoke();

        public override void Setup()
        {
            base.Setup();

            AddService(MockTestActor.Object);
        }

        [Test]
        public async Task WHEN_VirtualActor_does_not_exist_SHOULD_create_and_initialize_one()
        {
            //Act
            Sut.GetById<ITestActor>("myId");
            await Task.Delay(10); //delay because initialization happens asyncronousmly

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync("myId"));
        }

        [Test]
        public async Task WHEN_VirtualActor_does_exist_SHOULD_return_it()
        {
            //Arrange
            Sut.GetById<ITestActor>("myId");

            //Act
            Sut.GetById<ITestActor>("myId");
            await Task.Delay(10); //delay because initialization happens asyncronousmly

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync("myId"), Times.Once);
        }

        [Test]
        public async Task WHEN_different_VirtualActor_exists_SHOULD_create_new_one()
        {
            //Arrange
            Sut.GetById<ITestActor>("myId");

            //Act
            Sut.GetById<ITestActor>("newId");
            await Task.Delay(10); //delay because initialization happens asyncronousmly

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync(It.IsAny<string>()), Times.Exactly(2));
        }
    }
}
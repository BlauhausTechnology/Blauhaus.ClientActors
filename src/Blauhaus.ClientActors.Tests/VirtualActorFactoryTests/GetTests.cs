using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests._Base;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.ClientActors.VirtualActors;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.VirtualActorFactoryTests
{
    public class GetTests : BaseActorTest<VirtualActorFactory>
    {

        private MockBuilder<ITestVirtualActor> MockTestActor => AddMock<ITestVirtualActor>().Invoke();

        public override void Setup()
        {
            base.Setup();

            AddService(MockTestActor.Object);
        }

        [Test]
        public async Task WHEN_VirtualActor_does_not_exist_SHOULD_create_and_initialize_one()
        {
            //Act
            Sut.Get<ITestVirtualActor>();
            await Task.Delay(10); //delay because initialization happens asyncronousmly

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync());
        }

        [Test]
        public async Task WHEN_VirtualActor_does_exist_SHOULD_return_it()
        {
            //Arrange
            Sut.Get<ITestVirtualActor>();

            //Act
            Sut.Get<ITestVirtualActor>();
            await Task.Delay(10); //delay because initialization happens asyncronousmly

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync(), Times.Once);
        }
         
    }
}
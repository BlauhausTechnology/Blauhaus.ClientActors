using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests._Base;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.ClientActors.VirtualActors;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.VirtualActorFactoryTests
{
    public class UseTests : BaseActorTest<VirtualActorFactory>
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
            Sut.Use<ITestVirtualActor>();
            await Task.Delay(10); //delay because initialization happens asyncronousmly

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync());
        }

        [Test]
        public async Task WHEN_VirtualActor_has_been_used_before_SHOULD_still_create_new_one()
        {
            //Arrange
            Sut.Use<ITestVirtualActor>();

            //Act
            Sut.Use<ITestVirtualActor>();
            await Task.Delay(10); //delay because initialization happens asyncronousmly

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync(), Times.Exactly(2));
        }

        [Test]
        public async Task WHEN_VirtualActor_has_been_got_before_SHOULD_reuse_it()
        {
            //Arrange
            Sut.Get<ITestVirtualActor>();

            //Act
            Sut.Use<ITestVirtualActor>();
            await Task.Delay(10); //delay because initialization happens asyncronousmly

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync(), Times.Exactly(1));
        }
         
    }
}
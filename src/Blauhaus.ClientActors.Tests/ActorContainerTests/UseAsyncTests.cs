using System.Threading.Tasks;
using Blauhaus.ClientActors.Manager;
using Blauhaus.ClientActors.Tests._Base;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.ActorContainerTests
{
    public class UseAsyncTests : BaseActorTest<ActorContainer<ITestActor>>
    {
        protected MockBuilder<ITestActor> MockTestActor => AddMock<ITestActor>().Invoke();

        public override void Setup()
        {
            base.Setup();

            AddService(MockTestActor.Object); 
        }

        [Test]
        public async Task WHEN_Actor_does_not_yet_exist_SHOULD_create_and_initialize_one()
        {
            //Act
            var result = await Sut.UseAsync("myId");

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync("myId"));
            Assert.That(result, Is.EqualTo(MockTestActor.Object));
        }

        [Test]
        public async Task WHEN_Actor_has_been_got_before_SHOULD_not_recreate_it()
        {
            //Arrange
            await Sut.GetAsync("myId");

            //Act
            var result = await Sut.UseAsync("myId");

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync("myId"), Times.Exactly(1));
            Assert.That(result, Is.EqualTo(MockTestActor.Object));
        }

        
        [Test]
        public async Task WHEN_Actor_has_been_used_before_SHOULD_create_and_initialize_one_anyway()
        {
            //Arrange
            await Sut.UseAsync("myId");

            //Act
            var result = await Sut.UseAsync("myId");

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync("myId"), Times.Exactly(2));
            Assert.That(result, Is.EqualTo(MockTestActor.Object));
        }
         
    }
}
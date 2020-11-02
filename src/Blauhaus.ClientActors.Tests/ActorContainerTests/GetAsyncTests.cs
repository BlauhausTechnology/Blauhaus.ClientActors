using System.Threading.Tasks;
using Blauhaus.ClientActors.Manager;
using Blauhaus.ClientActors.Tests._Base;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.ActorContainerTests
{
    public class GetAsyncTests : BaseActorTest<ActorContainer<ITestActor>>
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
            var result = await Sut.GetAsync("myId");

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync("myId"));
            Assert.That(result, Is.EqualTo(MockTestActor.Object));
        }

        [Test]
        public async Task WHEN_Actor_does_exist_SHOULD_return_it()
        {
            //Arrange
            await Sut.GetAsync("myId");

            //Act
            var result = await Sut.GetAsync("myId");

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync("myId"), Times.Once);
            Assert.That(result, Is.EqualTo(MockTestActor.Object));
        }

        [Test]
        public async Task WHEN_Multiple_Ids_given_should_create_and_cache_new_and_just_return_existing()
        {
            //Arrange
            await Sut.GetAsync("1");
            await Sut.GetAsync("2");
            await Sut.UseAsync("7");

            //Act
            var result = await Sut.GetAsync(new []{"1", "2", "3"});

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync("1"), Times.Once);
            MockTestActor.Mock.Verify(x => x.InitializeAsync("2"), Times.Once);
            MockTestActor.Mock.Verify(x => x.InitializeAsync("3"), Times.Once);
            Assert.That(result.Count, Is.EqualTo(3));
        }
         
    }
}
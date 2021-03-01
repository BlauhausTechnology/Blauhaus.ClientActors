using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests.Containers.ActorContainerTests.Base;
using Moq;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.Containers.ActorContainerTests
{
    public class GetOneAsyncTests : BaseActorContainerTest
    {
       
        [Test]
        public async Task WHEN_Actor_does_not_yet_exist_SHOULD_create_and_initialize_one()
        {
            //Act
            var result = await Sut.GetOneAsync("myId");

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync("myId"));
            Assert.That(result, Is.EqualTo(MockTestActor.Object));
        }

        [Test]
        public async Task WHEN_Actor_does_exist_SHOULD_return_it()
        {
            //Arrange
            await Sut.GetOneAsync("myId");

            //Act
            var result = await Sut.GetOneAsync("myId");

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync("myId"), Times.Once);
            Assert.That(result, Is.EqualTo(MockTestActor.Object));
        }
         
    }
}
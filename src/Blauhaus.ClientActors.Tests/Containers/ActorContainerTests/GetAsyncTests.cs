using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests.Containers.ActorContainerTests.Base;
using Moq;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.Containers.ActorContainerTests
{
    public class GetAsyncTests : BaseActorContainerTest
    { 

        [Test]
        public async Task WHEN_Multiple_Ids_given_should_create_and_cache_new_and_just_return_existing()
        {
            //Arrange
            await Sut.GetOneAsync("1");
            await Sut.GetOneAsync("2");
            await Sut.UseOneAsync("7");

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
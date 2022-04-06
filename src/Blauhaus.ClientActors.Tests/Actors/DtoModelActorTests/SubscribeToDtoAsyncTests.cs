using Blauhaus.ClientActors.Tests.Actors.DtoModelActorTests.Base;
using NUnit.Framework;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.Common.TestHelpers.Extensions;
using Blauhaus.TestHelpers.Extensions;

namespace Blauhaus.ClientActors.Tests.Actors.DtoModelActorTests
{
    public class SubscribeToDtoAsyncTests : BaseDtoModelActorTest
    {
         
        [Test]
        public async Task SHOULD_load_and_cache_model()
        {
            //Arrange
            await Sut.InitializeAsync(Id);
            using var updates = await Sut.SubscribeToUpdatesAsync();
            var dto = new TestDto { Id = Id, RandomThing = "Hi Sailor" };

            //Act
            await Sut.SubscribeToDtoAsync();
            await MockDtoLoader.PublishMockSubscriptionAsync(dto);

            //Assert
            updates.WaitFor(x => x.Count == 2);
            //first update is random
            Assert.That(updates[1].RandomThing, Is.EqualTo("Hi Sailor"));
        }
    }
}
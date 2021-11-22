using Blauhaus.ClientActors.Tests.Actors.DtoModelActorTests.Base;
using NUnit.Framework;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.Common.TestHelpers.Extensions;
using Blauhaus.TestHelpers.Extensions;

namespace Blauhaus.ClientActors.Tests.Actors.DtoModelActorTests
{
    public class DtoLoaderUpdatedTests : BaseDtoModelActorTest
    {
         
        [Test]
        public async Task SHOULD_load_and_cached_model()
        {
            //Arrange
            await Sut.InitializeAsync(Id);
            using var updates = await Sut.SubscribeToUpdatesAsync();

            //Act
            var dto = new TestDto { Id = Id, RandomThing = "Hi Sailor" };
            await MockDtoLoader.PublishMockSubscriptionAsync(dto);

            //Assert
            updates.WaitFor(x => x.Count == 1);
            Assert.That(updates[0].RandomThing, Is.EqualTo("Hi Sailor"));
        }
    }
}
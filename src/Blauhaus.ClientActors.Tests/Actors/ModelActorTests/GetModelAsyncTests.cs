using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests._Base;
using Blauhaus.ClientActors.Tests.Suts;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.Actors.ModelActorTests
{
    public class GetModelAsyncTests : BaseActorTest<TestModelActor>
    {
        [Test]
        public async Task SHOULD_load_and_cached_model()
        {
            //Arrange
            var id = Guid.NewGuid();
            await Sut.InitializeAsync(id);

            //Act
            var resultOne = await Sut.GetModelAsync();
            var resultTwo = await Sut.GetModelAsync();

            //Assert
            Assert.That(resultTwo, Is.EqualTo(resultOne));
            Assert.That(resultOne.Id, Is.EqualTo(id));
        }
    }
}
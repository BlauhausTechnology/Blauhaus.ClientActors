using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests.Base;
using Blauhaus.ClientActors.Tests.Suts;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.Actors.ModelActorTests
{
    public class ReloadAsyncTests : BaseActorTest<TestModelActor>
    {
        [Test]
        public async Task SHOULD_clear_cached_model()
        {
            //Arrange
            var id = Guid.NewGuid();
            await Sut.InitializeAsync(id);
            var resultOne = await Sut.GetModelAsync();

            //Act
            await Sut.ReloadAsync();
            var resultTwo = await Sut.GetModelAsync();

            //Assert
            Assert.That(resultTwo, Is.Not.EqualTo(resultOne));
            Assert.That(resultOne.RandomThing, Is.Not.EqualTo(resultTwo.RandomThing));

            Assert.That(resultOne.Id, Is.EqualTo(id));
            Assert.That(resultOne.Id, Is.EqualTo(resultTwo.Id));
        }

        [Test]
        public async Task SHOULD_update_subscribers()
        {
            //Arrange
            var id = Guid.NewGuid();
            await Sut.InitializeAsync(id);
            var resultOne = await Sut.GetModelAsync();
            ITestModel resultTwo = null;
            await Sut.SubscribeAsync(testModel =>
            {
                resultTwo = testModel;
                return Task.CompletedTask;
            });

            //Act
            await Sut.ReloadAsync(); 

            //Assert
            Assert.That(resultTwo, Is.Not.EqualTo(resultOne));
            Assert.That(resultOne.RandomThing, Is.Not.EqualTo(resultTwo.RandomThing));

            Assert.That(resultOne.Id, Is.EqualTo(id));
            Assert.That(resultOne.Id, Is.EqualTo(resultTwo.Id));
        }

        
        [Test]
        public async Task SHOULD_not_update_subscribers_who_have_unsibscribed()
        {
            //Arrange
            var id = Guid.NewGuid();
            await Sut.InitializeAsync(id);
            ITestModel resultTwo = null;
            var token = await Sut.SubscribeAsync(testModel =>
            {
                resultTwo = testModel;
                return Task.CompletedTask;
            });
            resultTwo = null;
            token.Dispose();

            //Act
            await Sut.ReloadAsync(); 

            //Assert
            Assert.That(resultTwo == null);
        }
    }
}
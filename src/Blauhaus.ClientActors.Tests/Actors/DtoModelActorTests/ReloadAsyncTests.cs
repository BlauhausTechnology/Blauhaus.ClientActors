using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests.Actors.DtoModelActorTests.Base;
using Blauhaus.ClientActors.Tests.Base;
using Blauhaus.ClientActors.Tests.Suts;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.Actors.DtoModelActorTests
{
    public class ReloadAsyncTests : BaseDtoModelActorTest
    {
        [Test]
        public async Task SHOULD_clear_cached_model()
        {
            //Arrange
            await Sut.InitializeAsync(Id);
            var resultOne = await Sut.GetModelAsync();

            //Act
            await Sut.ReloadAsync();
            var resultTwo = await Sut.GetModelAsync();

            //Assert
            Assert.That(resultTwo, Is.Not.EqualTo(resultOne));
            Assert.That(resultOne.RandomThing, Is.Not.EqualTo(resultTwo.RandomThing));

            Assert.That(resultOne.Id, Is.EqualTo(Id));
            Assert.That(resultOne.Id, Is.EqualTo(resultTwo.Id));
        }

        [Test]
        public async Task SHOULD_update_subscribers()
        {
            //Arrange 
            await Sut.InitializeAsync(Id);
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
            Assert.That(resultOne.RandomThing, Is.Not.EqualTo(resultTwo!.RandomThing));

            Assert.That(resultOne.Id, Is.EqualTo(Id));
            Assert.That(resultOne.Id, Is.EqualTo(resultTwo.Id));
        }

        
        [Test]
        public async Task SHOULD_not_update_subscribers_who_have_unsibscribed()
        {
            //Arrange 
            await Sut.InitializeAsync(Id);
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
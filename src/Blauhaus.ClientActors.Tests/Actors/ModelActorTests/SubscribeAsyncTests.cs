using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests.Base;
using Blauhaus.ClientActors.Tests.Suts;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.Actors.ModelActorTests
{
    public class SubscribeAsyncTests : BaseActorTest<TestModelActor>
    {
 
        [Test]
        public async Task SHOULD_publishModel()
        {
            //Arrange
            var id = Guid.NewGuid();
            await Sut.InitializeAsync(id);
            ITestModel? result = null;

            //Act
            await Sut.SubscribeAsync(testModel =>
            {
                result = testModel;
                return Task.CompletedTask;
            });
            await Sut.UpdateSubscribersWithCurrentModelAsync();


            //Assert 
            Assert.That(result!.Id, Is.EqualTo(id)); 
        }

         
    }
}
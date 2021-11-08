using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests.Actors.DtoModelActorTests.Base;
using Blauhaus.ClientActors.Tests.Base;
using Blauhaus.ClientActors.Tests.Suts;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.Actors.DtoModelActorTests
{
    public class SubscribeAsyncTests : BaseDtoModelActorTest
    {
 
        [Test]
        public async Task SHOULD_publishModel()
        {
            //Arrange
            await Sut.InitializeAsync(Id);
            ITestModel? result = null;

            //Act
            await Sut.SubscribeAsync(testModel =>
            {
                result = testModel;
                return Task.CompletedTask;
            });
            await Sut.UpdateSubscribersWithCurrentModelAsync();


            //Assert 
            Assert.That(result!.Id, Is.EqualTo(Id)); 
        }

         
    }
}
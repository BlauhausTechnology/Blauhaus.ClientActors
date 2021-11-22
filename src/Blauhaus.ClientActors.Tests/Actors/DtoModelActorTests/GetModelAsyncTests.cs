﻿using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests.Actors.DtoModelActorTests.Base;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.Actors.DtoModelActorTests
{
    public class GetModelAsyncTests : BaseDtoModelActorTest
    {
 
        [Test]
        public async Task SHOULD_load_and_cached_model()
        {
            //Arrange
            await Sut.InitializeAsync(Id);

            //Act
            var resultOne = await Sut.GetModelAsync();
            var resultTwo = await Sut.GetModelAsync();

            //Assert
            Assert.That(resultTwo, Is.EqualTo(resultOne));
            Assert.That(resultOne.Id, Is.EqualTo(Id));
        }
    }
}
﻿using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests.Containers.ActorContainerTests.Base;
using Moq;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.Containers.ActorContainerTests
{
    public class UseAsyncTests : BaseActorContainerTest 
    {

        [Test]
        public async Task WHEN_Actor_does_not_yet_exist_SHOULD_create_and_initialize_one()
        {
            //Act
            var result = await Sut.UseOneAsync("myId");

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync("myId"));
            Assert.That(result, Is.EqualTo(MockTestActor.Object));
        }

        [Test]
        public async Task WHEN_Actor_has_been_got_before_SHOULD_not_recreate_it()
        {
            //Arrange
            await Sut.GetOneAsync("myId");

            //Act
            var result = await Sut.UseOneAsync("myId");

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync("myId"), Times.Exactly(1));
            Assert.That(result, Is.EqualTo(MockTestActor.Object));
        }

        
        [Test]
        public async Task WHEN_Actor_has_been_used_before_SHOULD_create_and_initialize_one_anyway()
        {
            //Arrange
            await Sut.UseOneAsync("myId");

            //Act
            var result = await Sut.UseOneAsync("myId");

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync("myId"), Times.Exactly(2));
            Assert.That(result, Is.EqualTo(MockTestActor.Object));
        }

        [Test]
        public async Task WHEN_Multiple_Ids_given_should_create_new_without_caching_and_return_existing()
        {
            //Arrange
            await Sut.GetOneAsync("1");
            await Sut.GetOneAsync("2");
            await Sut.UseOneAsync("7");

            //Act
            var result = await Sut.UseAsync(new []{"1", "2", "3"});

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync("1"), Times.Once);
            MockTestActor.Mock.Verify(x => x.InitializeAsync("2"), Times.Once);
            MockTestActor.Mock.Verify(x => x.InitializeAsync("3"), Times.Once);
            Assert.That(result.Count, Is.EqualTo(3));
            var cached = await Sut.GetActiveAsync();
            Assert.That(cached.Count, Is.EqualTo(2));

        }

         
    }
}
﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests.Containers.ActorContainerTests.Base;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.Ioc.TestHelpers;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.Containers.ActorContainerTests
{
    public class RemoveAllAsyncTests : BaseActorContainerTest 
    {


        [Test]
        public async Task SHOULD_dispose_and_remove_return_activated_actors()
        {
            //Arrange
            await Sut.GetOneAsync("1"); 
            await Sut.GetOneAsync("2"); 
            await Sut.GetOneAsync("3"); 

            //Act
            await Sut.RemoveAllAsync();

            //Assert
            var result = await Sut.GetActiveAsync();
            Assert.That(result.Count, Is.EqualTo(0)); 
            MockTestActor.Mock.Verify(x => x.DisposeAsync(), Times.Exactly(3));
        }

        [Test]
        public async Task WHEN_Ids_are_given_SHOULD_only_remove_and_dispose_actors_that_match()
        {
            //Arrange
            await Sut.GetOneAsync("1"); 
            await Sut.GetOneAsync("2"); 
            await Sut.GetOneAsync("3"); 
            await Sut.GetOneAsync("4"); 
            await Sut.GetOneAsync("5"); 

            //Act
            await Sut.RemoveAsync(new []{"3", "4"});

            //Assert
            var result = await Sut.GetActiveAsync();
            Assert.That(result.Count, Is.EqualTo(3)); 
            MockTestActor.Mock.Verify(x => x.DisposeAsync(), Times.Exactly(2));
        }
        
        
        [Test]
        public async Task WHEN_Id_is_given_SHOULD_only_remove_and_dispose_that_actor()
        {
            //Arrange
            await Sut.GetOneAsync("1"); 
            await Sut.GetOneAsync("2"); 
            await Sut.GetOneAsync("3"); 
            await Sut.GetOneAsync("4"); 
            await Sut.GetOneAsync("5"); 

            //Act
            await Sut.RemoveAsync("3");

            //Assert
            var result = await Sut.GetActiveAsync();
            Assert.That(result.Count, Is.EqualTo(4)); 
            MockTestActor.Mock.Verify(x => x.DisposeAsync(), Times.Exactly(1));
        }

        [Test]
        public async Task WHEN_predicate_is_given_SHOULD_only_remove_and_dispose_actors_that_match()
        {
            //Arrange
            var mockServiceLocator = new ServiceLocatorMockBuilder().Where_Resolve_returns_sequence(
                new List<ITestActor>
                {
                    new MockBuilder<ITestActor>().With(x => x.ExtraProperty, "adrian").Object,
                    new MockBuilder<ITestActor>().With(x => x.ExtraProperty, "bob").Object,
                    new MockBuilder<ITestActor>().With(x => x.ExtraProperty, "cyril").Object,
                    new MockBuilder<ITestActor>().With(x => x.ExtraProperty, "domino").Object,
                });
            AddService(mockServiceLocator.Object);
            await Sut.GetOneAsync("1"); 
            await Sut.GetOneAsync("2"); 
            await Sut.GetOneAsync("3"); 
            await Sut.GetOneAsync("4");  

            //Act
            await Sut.RemoveAsync(x => x.ExtraProperty.Contains("d"));

            //Assert
            var result = await Sut.GetActiveAsync();
            Assert.That(result.Count, Is.EqualTo(2));  
        }
    }
}
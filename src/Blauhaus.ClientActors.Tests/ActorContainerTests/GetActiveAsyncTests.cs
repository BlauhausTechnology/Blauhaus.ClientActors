using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests._Base;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.Ioc.TestHelpers;
using Blauhaus.TestHelpers.MockBuilders;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.ActorContainerTests
{
    public class GetActiveAsyncTests : BaseActorTest<ActorContainer<ITestActor>>
    {
        protected MockBuilder<ITestActor> MockTestActor => AddMock<ITestActor>().Invoke();

        public override void Setup()
        {
            base.Setup();

            AddService(MockTestActor.Object); 
        }

        [Test]
        public async Task SHOULD_return_activated_actors()
        {
            //Arrange
            await Sut.GetAsync("1"); 
            await Sut.GetAsync("2"); 
            await Sut.GetAsync("3"); 
            await Sut.UseAsync("4"); 

            //Act
            var result = await Sut.GetActiveAsync();

            //Assert
            Assert.That(result.Count, Is.EqualTo(3)); 
        }

        [Test]
        public async Task WHN_Ids_are_given_SHOULD_only_return_actors_that_match()
        {
            //Arrange
            await Sut.GetAsync("1"); 
            await Sut.GetAsync("2"); 
            await Sut.GetAsync("3"); 
            await Sut.GetAsync("4"); 
            await Sut.GetAsync("5"); 
            await Sut.UseAsync("6"); 

            //Act
            var result = await Sut.GetActiveAsync(new []{"3", "4"});

            //Assert
            Assert.That(result.Count, Is.EqualTo(2)); 
        }

        [Test]
        public async Task WHEN_predicate_is_given_SHOULD_only_return_actors_that_match()
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
            await Sut.GetAsync("1"); 
            await Sut.GetAsync("2"); 
            await Sut.GetAsync("3"); 
            await Sut.GetAsync("4");  

            //Act
            var result = await Sut.GetActiveAsync(x => x.ExtraProperty.Contains("d"));

            //Assert
            Assert.That(result.Count, Is.EqualTo(2)); 
            Assert.That(result.FirstOrDefault(x => x.ExtraProperty == "adrian"), Is.Not.Null);
            Assert.That(result.FirstOrDefault(x => x.ExtraProperty == "domino"), Is.Not.Null);
        }
    }
}
using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests._Base;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.ClientActors.VirtualActors;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.ActorFactoryTests
{
    public class GetByIdTests : BaseActorTest<VirtualActorFactory>
    {

        private MockBuilder<ITestActor> MockTestActor => AddMock<ITestActor>().Invoke();
        private MockBuilder<IGenericTestActor<int>> MockGenericIntTestActor => AddMock<IGenericTestActor<int>>().Invoke();
        private MockBuilder<IGenericTestActor<string>> MockGenericStringTestActor => AddMock<IGenericTestActor<string>>().Invoke();

        public override void Setup()
        {
            base.Setup();

            AddService(MockTestActor.Object);
            AddService(MockGenericStringTestActor.Object);
            AddService(MockGenericIntTestActor.Object);
        }

        [Test]
        public async Task WHEN_VirtualActor_does_not_exist_SHOULD_create_and_initialize_one()
        {
            //Act
            Sut.GetById<ITestActor>("myId");
            await Task.Delay(10); //delay because initialization happens asyncronousmly

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync("myId"));
        }

        [Test]
        public async Task WHEN_VirtualActor_does_exist_SHOULD_return_it()
        {
            //Arrange
            Sut.GetById<ITestActor>("myId");

            //Act
            Sut.GetById<ITestActor>("myId");
            await Task.Delay(10); //delay because initialization happens asyncronousmly

            //Assert
            MockTestActor.Mock.Verify(x => x.InitializeAsync("myId"), Times.Once);
        }
         

        [Test]
        public async Task SHOULD_treat_different_generics_with_same_id_as_different()
        {
            //Arrange
            Sut.GetById<IGenericTestActor<int>>("myId");

            //Act
            Sut.GetById<IGenericTestActor<string>>("myId");
            await Task.Delay(10); //delay because initialization happens asyncronousmly

            //Assert
            MockGenericIntTestActor.Mock.Verify(x => x.InitializeAsync(It.IsAny<string>()), Times.Exactly(1));
            MockGenericStringTestActor.Mock.Verify(x => x.InitializeAsync(It.IsAny<string>()), Times.Exactly(1));
            Assert.That(Sut.GetActive<IGenericTestActor<int>>().Count, Is.EqualTo(1));
            Assert.That(Sut.GetActive<IGenericTestActor<string>>().Count, Is.EqualTo(1));
        }

        [Test]
        public async Task SHOULD_treat_same_generics_with_same_id_as_same()
        {
            //Arrange
            Sut.GetById<IGenericTestActor<int>>("myId");

            //Act
            Sut.GetById<IGenericTestActor<int>>("myId");
            await Task.Delay(10); //delay because initialization happens asyncronousmly

            //Assert
            MockGenericIntTestActor.Mock.Verify(x => x.InitializeAsync(It.IsAny<string>()), Times.Exactly(1));
            MockGenericStringTestActor.Mock.Verify(x => x.InitializeAsync(It.IsAny<string>()), Times.Exactly(0));
            Assert.That(Sut.GetActive<IGenericTestActor<int>>().Count, Is.EqualTo(1));
            Assert.That(Sut.GetActive<IGenericTestActor<string>>().Count, Is.EqualTo(0));
        }
    }
}
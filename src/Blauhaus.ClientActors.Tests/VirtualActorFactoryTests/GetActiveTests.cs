using Blauhaus.ClientActors.Tests._Base;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.ClientActors.VirtualActors;
using Blauhaus.TestHelpers.MockBuilders;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.VirtualActorFactoryTests
{
    public class GetActiveTests : BaseActorTest<VirtualActorFactory>
    {

        private MockBuilder<ITestVirtualActor> MockTestActor => AddMock<ITestVirtualActor>().Invoke();
        private MockBuilder<IOtherTestActor> MockOtherTestActor => AddMock<IOtherTestActor>().Invoke();

        public override void Setup()
        {
            base.Setup();

            AddService(MockTestActor.Object);
            AddService(MockOtherTestActor.Object);
        }
          
        [Test]
        public void SHOULD_return_activated_actors_of_requested_type()
        {
            //Arrange
            Sut.GetById<ITestVirtualActor>("1");
            Sut.GetById<IOtherTestActor>("a");
            Sut.GetById<ITestVirtualActor>("2");
            Sut.GetById<ITestVirtualActor>("3");

            //Act
            var result = Sut.GetActive<ITestVirtualActor>();

            //Assert
            Assert.That(result.Count, Is.EqualTo(3)); 

        }

        [Test]
        public void WHEN_prodicate_is_given_SHOULD_return_activated_actors_of_requested_type_with_matching_ids()
        {
            //Arrange
            Sut.GetById<ITestVirtualActor>("1");
            Sut.GetById<IOtherTestActor>("a");
            Sut.GetById<ITestVirtualActor>("2");
            Sut.GetById<ITestVirtualActor>("3");

            //Act
            var result = Sut.GetActive<ITestVirtualActor>();

            //Assert
            Assert.That(result.Count, Is.EqualTo(3)); 

        }
         
    }
}
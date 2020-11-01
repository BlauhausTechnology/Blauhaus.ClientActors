using System.Linq;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Tests._Base;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.ClientActors.VirtualActors;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.ActorFactoryTests
{
    public class GetActiveTests : BaseActorTest<VirtualActorFactory>
    {

        private MockBuilder<ITestActor> MockTestActor => AddMock<ITestActor>().Invoke();
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
            Sut.GetById<ITestActor>("1");
            Sut.GetById<IOtherTestActor>("a");
            Sut.GetById<ITestActor>("2");
            Sut.GetById<ITestActor>("3");

            //Act
            var result = Sut.GetActive<ITestActor>();

            //Assert
            Assert.That(result.Count, Is.EqualTo(3)); 

        }
         
    }
}
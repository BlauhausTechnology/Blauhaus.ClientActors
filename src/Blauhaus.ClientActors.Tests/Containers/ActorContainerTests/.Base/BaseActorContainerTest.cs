﻿using Blauhaus.ClientActors.Containers;
using Blauhaus.ClientActors.Tests._Base;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.TestHelpers.MockBuilders;

namespace Blauhaus.ClientActors.Tests.Containers.ActorContainerTests.Base
{
    public abstract class BaseActorContainerTest : BaseActorTest<ActorContainer<ITestActor, string>>
    {
        protected MockBuilder<ITestActor> MockTestActor => AddMock<ITestActor>().Invoke();

        public override void Setup()
        {
            base.Setup();

            AddService(MockTestActor.Object); 
        }
    }
}
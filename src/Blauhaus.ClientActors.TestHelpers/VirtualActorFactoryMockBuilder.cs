using System.Collections.Generic;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.ClientActors.TestHelpers
{
    public class VirtualActorFactoryMockBuilder : BaseMockBuilder<VirtualActorFactoryMockBuilder, IVirtualActorFactory>
    {
         

        public VirtualActorFactoryMockBuilder Where_Get_returns<TActor>(IVirtualActor<TActor> result) where TActor : class, IInitialize
        {
            Mock.Setup(x => x.Get<TActor>()).Returns(result);
            return this;
        }

        public VirtualActorFactoryMockBuilder Where_Use_returns<TActor>(IVirtualActor<TActor> result) where TActor : class, IInitialize
        {
            Mock.Setup(x => x.Use<TActor>()).Returns(result);
            return this;
        }

        public VirtualActorFactoryMockBuilder Where_UseById_returns<TActor>(IVirtualActor<TActor> result, string id = null) where TActor : class, IInitializeById
        {
            if (id == null)
            {
                Mock.Setup(x => x.UseById<TActor>(It.IsAny<string>())).Returns(result);
            }
            else
            {
                Mock.Setup(x => x.UseById<TActor>(id)).Returns(result);
            }
            return this;
        }

        
        public VirtualActorFactoryMockBuilder Where_GetById_returns<TActor>(IVirtualActor<TActor> result, string id = null) where TActor : class, IInitializeById
        {
            if (id == null)
            {
                Mock.Setup(x => x.GetById<TActor>(It.IsAny<string>())).Returns(result);
            }
            else
            {
                Mock.Setup(x => x.GetById<TActor>(id)).Returns(result);
            }
            return this;
        }

        public VirtualActorFactoryMockBuilder Where_GetActive_returns<TActor>(IReadOnlyList<IVirtualActor<TActor>> result)  
        {
            Mock.Setup(x => x.GetActive<TActor>()).Returns(result);
            return this;
        }

    }
}
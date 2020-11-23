using System;
using System.Collections.Generic;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.ClientActors.TestHelpers
{
    public class ActorContainerMockBuilder<TActor> : BaseMockBuilder<ActorContainerMockBuilder<TActor>, IActorContainer<TActor>> where TActor : class, IActor
    {
        private static List<TActor> _empty = new List<TActor>();
        public ActorContainerMockBuilder()
        {
            Where_GetActiveAsync_returns(_empty);
            Where_GetAsync_returns(_empty);
            Where_GetAsync_returns(new Mock<TActor>().Object);
            Where_UseAsync_returns(_empty);
            Where_UseAsync_returns(new Mock<TActor>().Object);
        }

        public ActorContainerMockBuilder<TActor> Where_GetAsync_returns(TActor actor, string id = null)
        {
            if (id == null)
            {
                Mock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(actor);
                Mock.Setup(x => x.GetAsync(It.IsAny<IEnumerable<string>>()))
                    .ReturnsAsync(new List<TActor>{actor});
            }
            else
            {
                Mock.Setup(x => x.GetAsync(id)).ReturnsAsync(actor);
            }
            return this;
        }
        public ActorContainerMockBuilder<TActor> Where_GetAsync_returns(IReadOnlyList<TActor> actors)
        {
            Mock.Setup(x => x.GetAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(actors);
            return this;
        }

        public ActorContainerMockBuilder<TActor> Where_UseAsync_returns(TActor actor, string id = null)
        {
            if (id == null)
            {
                Mock.Setup(x => x.UseAsync(It.IsAny<string>())).ReturnsAsync(actor);
                Mock.Setup(x => x.UseAsync(It.IsAny<IEnumerable<string>>()))
                    .ReturnsAsync(new List<TActor>{actor});
            }
            else
            {
                Mock.Setup(x => x.UseAsync(id)).ReturnsAsync(actor);
            }
            return this;
        }
        public ActorContainerMockBuilder<TActor> Where_UseAsync_returns(IReadOnlyList<TActor> actors)
        {
            Mock.Setup(x => x.UseAsync(It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(actors);
            return this;
        }

        public ActorContainerMockBuilder<TActor> Where_GetActiveAsync_returns(IReadOnlyList<TActor> actors)
        {
            Mock.Setup(x => x.GetActiveAsync()).ReturnsAsync(actors);
            Mock.Setup(x => x.GetActiveAsync(It.IsAny<Func<TActor, bool>>())).ReturnsAsync(actors);
            Mock.Setup(x => x.GetActiveAsync(It.IsAny<IEnumerable<string>>())).ReturnsAsync(actors);
            return this;
        }
    }
}
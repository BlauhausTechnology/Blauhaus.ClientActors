using System;
using System.Collections.Generic;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.ClientActors.TestHelpers
{

    public class ActorContainerMockBuilder<TActor, TId> : BaseActorContainerMockBuilder<ActorContainerMockBuilder<TActor, TId>, IActorContainer<TActor, TId>, TActor, TId>
        where TActor : class, IActor<TId> 
    {
    }

    public abstract class BaseActorContainerMockBuilder<TBuilder, TContainer, TActor, TId> : BaseMockBuilder<TBuilder, TContainer>
        where TActor : class, IActor<TId>
        where TBuilder : BaseActorContainerMockBuilder<TBuilder, TContainer, TActor, TId>
        where TContainer : class, IActorContainer<TActor, TId>
    {
        private static readonly List<TActor> _empty = new List<TActor>();

        protected BaseActorContainerMockBuilder()
        {
            Where_GetActiveAsync_returns(_empty);
            Where_GetAsync_returns(_empty);
            Where_GetAsync_returns(new Mock<TActor>().Object);
            Where_UseAsync_returns(_empty);
            Where_UseAsync_returns(new Mock<TActor>().Object);
        }

        public TBuilder Where_GetAsync_returns(TActor actor, TId id = default)
        {
            if (id == null)
            {
                Mock.Setup(x => x.GetAsync(It.IsAny<TId>())).ReturnsAsync(actor);
                Mock.Setup(x => x.GetAsync(It.IsAny<IEnumerable<TId>>()))
                    .ReturnsAsync(new List<TActor>{actor});
            }
            else
            {
                Mock.Setup(x => x.GetAsync(id)).ReturnsAsync(actor);
            }
            return (TBuilder) this;
        }
        public TBuilder Where_GetAsync_returns(IReadOnlyList<TActor> actors)
        {
            Mock.Setup(x => x.GetAsync(It.IsAny<IEnumerable<TId>>()))
                .ReturnsAsync(actors);
            return (TBuilder) this;
        }

        public TBuilder Where_UseAsync_returns(TActor actor, TId id = default)
        {
            if (id == null)
            {
                Mock.Setup(x => x.UseAsync(It.IsAny<TId>())).ReturnsAsync(actor);
                Mock.Setup(x => x.UseAsync(It.IsAny<IEnumerable<TId>>()))
                    .ReturnsAsync(new List<TActor>{actor});
            }
            else
            {
                Mock.Setup(x => x.UseAsync(id)).ReturnsAsync(actor);
            }
            return (TBuilder) this;
        }
        public TBuilder Where_UseAsync_returns(IReadOnlyList<TActor> actors)
        {
            Mock.Setup(x => x.UseAsync(It.IsAny<IEnumerable<TId>>()))
                .ReturnsAsync(actors);
            return (TBuilder) this;
        }

        public TBuilder Where_GetActiveAsync_returns(IReadOnlyList<TActor> actors)
        {
            Mock.Setup(x => x.GetActiveAsync()).ReturnsAsync(actors);
            Mock.Setup(x => x.GetActiveAsync(It.IsAny<Func<TActor, bool>>())).ReturnsAsync(actors);
            Mock.Setup(x => x.GetActiveAsync(It.IsAny<IEnumerable<TId>>())).ReturnsAsync(actors);
            return (TBuilder) this;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
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
        private static readonly List<TActor> Empty = new List<TActor>();

        protected BaseActorContainerMockBuilder()
        {
            Where_GetActiveAsync_returns(Empty);
            Where_GetAsync_returns(Empty);
            Where_GetOneAsync_returns(new Mock<TActor>().Object);
            Where_UseAsync_returns(Empty);
            Where_UseOneAsync_returns(new Mock<TActor>().Object);
        }

        public TBuilder Where_GetOneAsync_returns(TActor actor, TId id = default)
        {
            if (id == null)
            {
                Mock.Setup(x => x.GetOneAsync(It.IsAny<TId>())).ReturnsAsync(actor); 
            }
            else
            {
                Mock.Setup(x => x.GetOneAsync(id)).ReturnsAsync(actor);
            }
            return (TBuilder) this;
        }
        public TBuilder Where_GetAsync_returns(IReadOnlyList<TActor> actors)
        {
            Mock.Setup(x => x.GetAsync(It.IsAny<IEnumerable<TId>>()))
                .ReturnsAsync(actors);
            return (TBuilder) this;
        }
        public TBuilder Where_GetAsync_returns(TActor actor)
        {
            Mock.Setup(x => x.GetAsync(It.IsAny<IEnumerable<TId>>()))
                .ReturnsAsync(new List<TActor>{actor});
            return (TBuilder) this;
        }

        public TBuilder Where_UseOneAsync_returns(TActor actor, TId id = default)
        {
            if (id == null)
            {
                Mock.Setup(x => x.UseOneAsync(It.IsAny<TId>())).ReturnsAsync(actor); 
            }
            else
            {
                Mock.Setup(x => x.UseOneAsync(id)).ReturnsAsync(actor);
            }
            return (TBuilder) this;
        }
        public TBuilder Where_UseAsync_returns(IReadOnlyList<TActor> actors)
        {
            Mock.Setup(x => x.UseAsync(It.IsAny<IEnumerable<TId>>()))
                .ReturnsAsync(actors);
            return (TBuilder) this;
        }
        public TBuilder Where_UseAsync_returns(TActor actor)
        {
            Mock.Setup(x => x.UseAsync(It.IsAny<IEnumerable<TId>>()))
                .ReturnsAsync(new List<TActor>{actor});
            return (TBuilder) this;
        }

        public TBuilder Where_GetActiveAsync_returns(IReadOnlyList<TActor> actors)
        {
            Mock.Setup(x => x.GetActiveAsync()).ReturnsAsync(actors);
            Mock.Setup(x => x.GetActiveAsync(It.IsAny<Func<TActor, bool>>())).ReturnsAsync(actors);
            Mock.Setup(x => x.GetActiveAsync(It.IsAny<IEnumerable<TId>>())).ReturnsAsync(actors);
            return (TBuilder) this;
        }


        public void Verify_GetOneAsync(TId id)
        {
            Mock.Verify(x => x.GetOneAsync(id));
        }

        public void Verify_GetAsync(TId id)
        {
            Mock.Verify(x => x.GetAsync(It.Is<IEnumerable<TId>>(y => y.Contains(id))));
        }


    }
}
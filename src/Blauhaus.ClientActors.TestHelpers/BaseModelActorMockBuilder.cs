using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Utils.Contracts;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.ClientActors.TestHelpers
{
    public abstract class BaseModelActorMockBuilder<TBuilder, TActor, TId, TModel> : BaseMockBuilder<TBuilder, TActor> 
        where TBuilder : BaseModelActorMockBuilder<TBuilder, TActor, TId, TModel> 
        where TActor : class, IModelActor<TId, TModel>
        where TModel : IId<TId>
    {
        
        private readonly List<Func<TModel, Task>> _handlers = new List<Func<TModel, Task>>();

        public TBuilder Where_GetModelAsync_returns(TModel model)
        {
            Mock.Setup(x => x.GetModelAsync())
                .ReturnsAsync(model);
            return (TBuilder) this;
        }
         

        public Mock<IDisposable> AllowMockSubscriptions()
        {
            var mockToken = new Mock<IDisposable>();

            Mock.Setup(x => x.SubscribeAsync(It.IsAny<Func<TModel, Task>>()))
                .Callback((Func<TModel, Task> handler) =>
                {
                    _handlers.Add(handler);
                }).ReturnsAsync(mockToken.Object);

            return mockToken;
        }

        public async Task PublishMockSubscriptionAsync(TModel model)
        {
            foreach (var handler in _handlers)
            {
                await handler.Invoke(model);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Utils.Contracts;
using Blauhaus.TestHelpers.Builders._Base;
using Moq;

namespace Blauhaus.ClientActors.TestHelpers
{
    public class ModelActorContainerMockBuilder<TActor, TId, TModel> : BaseActorContainerMockBuilder<ModelActorContainerMockBuilder<TActor, TId, TModel>, IModelActorContainer<TActor, TId, TModel>, TActor, TId>
        where TActor : class, IModelActor<TId, TModel>
        where TModel : IHasId<TId>
    {
        private readonly List<Func<TModel, Task>> _handlers = new List<Func<TModel, Task>>();

        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_GetModelAsync_returns(TModel model)
        {
            Mock.Setup(x => x.GetModelAsync(It.IsAny<TId>())).ReturnsAsync(model);

            return this;
        }

        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_GetModelAsync_returns<TModelBuilder>(TModelBuilder modelBuilder) where TModelBuilder : IBuilder<TModelBuilder, TModel>
        {
            Mock.Setup(x => x.GetModelAsync(It.IsAny<TId>())).ReturnsAsync(() => modelBuilder.Object);

            return this;
        }

        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_GetModelAsync_returns(Func<TModel> model) 
        {
            Mock.Setup(x => x.GetModelAsync(It.IsAny<TId>())).ReturnsAsync(model.Invoke);

            return this;
        }

        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_GetModelAsync_returns(TModel model, TId id)
        {
            Mock.Setup(x => x.GetModelAsync(id))
                .ReturnsAsync(model);
            return this;
        }
        
        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_GetModelAsync_returns(Func<TModel> model, TId id)
        {
            Mock.Setup(x => x.GetModelAsync(id))
                .ReturnsAsync(model.Invoke);
            return this;
        }
        
        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_GetModelsAsync_returns(TModel model)
        {
            Mock.Setup(x => x.GetModelsAsync(It.IsAny<IEnumerable<TId>>()))
                .ReturnsAsync(new List<TModel>{model});
            return this;
        }
        
        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_GetModelsAsync_returns(Func<TModel> model)
        {
            Mock.Setup(x => x.GetModelsAsync(It.IsAny<IEnumerable<TId>>()))
                .ReturnsAsync(new List<TModel>{model.Invoke()});
            return this;
        }
        
        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_GetModelsAsync_returns(IReadOnlyList<TModel> models)
        {
            Mock.Setup(x => x.GetModelsAsync(It.IsAny<IEnumerable<TId>>()))
                .ReturnsAsync(models);
            return this;
        }

        public Mock<IDisposable> Where_SubscribeToModelAsync_publishes_immediately(TId id, TModel update)
        {
            var mockToken = new Mock<IDisposable>();

            Mock.Setup(x => x.SubscribeToModelAsync(id, It.IsAny<Func<TModel, Task>>()))
                .Callback((TId givenId, Func<TModel, Task> handler) =>
                {
                    handler.Invoke(update);
                }).ReturnsAsync(mockToken.Object);

            return mockToken;
        }

        public Mock<IDisposable> Where_SubscribeToModelAsync_publishes_sequence(TId id, IEnumerable<TModel> updates)
        {
            var mockToken = new Mock<IDisposable>();
            var queue = new Queue<TModel>(updates);

            Mock.Setup(x => x.SubscribeToModelAsync(id, It.IsAny<Func<TModel, Task>>()))
                .Callback((TId givenId, Func<TModel, Task> handler) =>
                {
                    handler.Invoke(queue.Dequeue());
                }).ReturnsAsync(mockToken.Object);

            return mockToken;
        }
        
        public Mock<IDisposable> AllowMockSubscriptions(TId id)
        {
            var mockToken = new Mock<IDisposable>();

            Mock.Setup(x => x.SubscribeToModelAsync(id, It.IsAny<Func<TModel, Task>>()))
                .Callback((TId givenId, Func<TModel, Task> handler) =>
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
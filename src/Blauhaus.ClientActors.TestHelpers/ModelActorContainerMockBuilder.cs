using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Abstractions;
using Blauhaus.TestHelpers.Builders._Base;
using Moq;

namespace Blauhaus.ClientActors.TestHelpers
{
    public class ModelActorContainerMockBuilder<TActor, TId, TModel> : BaseActorContainerMockBuilder<ModelActorContainerMockBuilder<TActor, TId, TModel>, IModelActorContainer<TActor, TId, TModel>, TActor, TId>
        where TActor : class, IModelActor<TId, TModel>
        where TModel : IHasId<TId>
    {
        private readonly List<Func<TModel, Task>> _handlers = new List<Func<TModel, Task>>();

        public Mock<IDisposable> MockToken { get; } = new Mock<IDisposable>();

        public ModelActorContainerMockBuilder()
        {
            Mock.Setup(x => x.SubscribeToModelAsync(It.IsAny<TId>(), It.IsAny<Func<TModel, Task>>()))
                .Callback((TId givenId, Func<TModel, Task> handler) =>
                {
                    _handlers.Add(handler); 
                }).ReturnsAsync(MockToken.Object);
        }
        
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
        
        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_GetModelsAsync_returns(Func<TModel> modelFactory)
        {
            Mock.Setup(x => x.GetModelsAsync(It.IsAny<IEnumerable<TId>>()))
                .ReturnsAsync(new List<TModel>{modelFactory.Invoke()});
            return this;
        }
        
        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_GetModelsAsync_returns(IReadOnlyList<TModel> models)
        {
            Mock.Setup(x => x.GetModelsAsync(It.IsAny<IEnumerable<TId>>()))
                .ReturnsAsync(models);
            return this;
        }

        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_SubscribeToModelAsync_publishes_immediately(TModel update, TId id)
        {
            Mock.Setup(x => x.SubscribeToModelAsync(id, It.IsAny<Func<TModel, Task>>()))
                .Callback((TId givenId, Func<TModel, Task> handler) =>
                {
                    _handlers.Add(handler);
                    handler.Invoke(update);
                }).ReturnsAsync(MockToken.Object);

            return this;
        }
        
        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_SubscribeToModelAsync_publishes_immediately(Func<TModel> modelFactory, TId id)
        { 
            Mock.Setup(x => x.SubscribeToModelAsync(id, It.IsAny<Func<TModel, Task>>()))
                .Callback((TId givenId, Func<TModel, Task> handler) =>
                {
                    _handlers.Add(handler);
                    handler.Invoke(modelFactory.Invoke());
                }).ReturnsAsync(MockToken.Object);

            return this;
        }

        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_SubscribeToModelAsync_publishes_sequence(IEnumerable<TModel> updates, TId id)
        { 
            var queue = new Queue<TModel>(updates);

            Mock.Setup(x => x.SubscribeToModelAsync(id, It.IsAny<Func<TModel, Task>>()))
                .Callback((TId givenId, Func<TModel, Task> handler) =>
                {
                    _handlers.Add(handler);
                    handler.Invoke(queue.Dequeue());
                }).ReturnsAsync(MockToken.Object);

            return this;
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
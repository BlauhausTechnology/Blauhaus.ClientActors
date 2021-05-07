using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Abstractions;
using Blauhaus.TestHelpers.Builders.Base;
using Moq;

namespace Blauhaus.ClientActors.TestHelpers
{
    public class ModelActorContainerMockBuilder<TActor, TId, TModel> : BaseActorContainerMockBuilder<ModelActorContainerMockBuilder<TActor, TId, TModel>, IModelActorContainer<TActor, TId, TModel>, TActor, TId>
        where TActor : class, IModelActor<TId, TModel>
        where TModel : IHasId<TId>
    {
        private readonly List<Func<TModel, Task>> _handlers = new List<Func<TModel, Task>>();
        private readonly List<Func<TModel, Task>> _activeModelHandlers = new List<Func<TModel, Task>>();

        public Mock<IDisposable> MockModelToken { get; } = new Mock<IDisposable>();
        public Mock<IDisposable> MockActiveModelsToken { get; } = new Mock<IDisposable>();

        public ModelActorContainerMockBuilder()
        {
            Mock.Setup(x => x.SubscribeToModelAsync(It.IsAny<TId>(), It.IsAny<Func<TModel, Task>>()))
                .Callback((TId givenId, Func<TModel, Task> handler) =>
                {
                    _handlers.Add(handler); 
                }).ReturnsAsync(MockModelToken.Object);

            Mock.Setup(x => x.SubscribeToActiveModelsAsync(It.IsAny<Func<TModel, Task>>()))
                .Callback((Func<TModel, Task> handler) =>
                {
                    _activeModelHandlers.Add(handler); 
                }).ReturnsAsync(MockModelToken.Object);
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

        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_GetActiveModelsAsync_returns(Func<TModel> modelFactory)
        {
            Mock.Setup(x => x.GetActiveModelsAsync())
                .ReturnsAsync(new List<TModel>{modelFactory.Invoke()});
            return this;
        }
        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_GetActiveModelsAsync_returns(IReadOnlyList<TModel> models)
        {
            Mock.Setup(x => x.GetActiveModelsAsync())
                .ReturnsAsync(models);
            return this;
        }
        
        
        public async Task PublishMockActiveModelSubscriptionAsync(TModel model)
        {
            foreach (var handler in _activeModelHandlers)
            {
                await handler.Invoke(model);
            }
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
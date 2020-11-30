using System;
using System.Collections.Generic;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Utils.Contracts;
using Moq;

namespace Blauhaus.ClientActors.TestHelpers
{
    public class ModelActorContainerMockBuilder<TActor, TId, TModel> : BaseActorContainerMockBuilder<ModelActorContainerMockBuilder<TActor, TId, TModel>, IModelActorContainer<TActor, TId, TModel>, TActor, TId>
        where TActor : class, IModelActor<TId, TModel>
        where TModel : IHasId<TId>
    {
        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_GetModelAsync_returns(TModel model)
        {
            Mock.Setup(x => x.GetModelAsync(It.IsAny<TId>())).ReturnsAsync(model);

            return this;
        }

        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_GetModelAsync_returns(TModel model, TId id)
        {
            Mock.Setup(x => x.GetModelAsync(id))
                .ReturnsAsync(model);
            return this;
        }
        
        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_GetModelsAsync_returns(TModel model)
        {
            Mock.Setup(x => x.GetModelsAsync(It.IsAny<IEnumerable<TId>>()))
                .ReturnsAsync(new List<TModel>{model});
            return this;
        }
        
        public ModelActorContainerMockBuilder<TActor, TId, TModel> Where_GetModelsAsync_returns(IReadOnlyList<TModel> models)
        {
            Mock.Setup(x => x.GetModelsAsync(It.IsAny<IEnumerable<TId>>()))
                .ReturnsAsync(models);
            return this;
        }
    }
}
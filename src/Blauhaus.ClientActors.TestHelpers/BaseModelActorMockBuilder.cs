using System;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Abstractions;
using Blauhaus.Common.TestHelpers.MockBuilders;
using Blauhaus.Domain.Abstractions.Actors;
using Moq;

namespace Blauhaus.ClientActors.TestHelpers
{
    public abstract class BaseModelActorMockBuilder<TBuilder, TActor, TModel, TId> : BaseAsyncPublisherMockBuilder<TBuilder, TActor, TModel>
        where TBuilder : BaseModelActorMockBuilder<TBuilder, TActor, TModel, TId>
        where TActor : class, IModelActor<TModel, TId>
        where TModel : IHasId<TId>
    {
        public TBuilder Where_GetModelAsync_returns(TModel model)
        {
            Mock.Setup(x => x.GetModelAsync())
                .ReturnsAsync(() => model);
            return (TBuilder) this;
        }
        
        public TBuilder Where_GetModelAsync_returns(Func<TModel> model)
        {
            Mock.Setup(x => x.GetModelAsync())
                .ReturnsAsync(model.Invoke);
            return (TBuilder) this;
        }

    }
         
}
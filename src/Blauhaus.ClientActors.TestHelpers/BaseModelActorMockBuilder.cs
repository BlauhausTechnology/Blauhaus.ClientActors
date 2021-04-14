using System;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Abstractions;
using Blauhaus.Common.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.ClientActors.TestHelpers
{
    public abstract class BaseModelActorMockBuilder<TBuilder, TActor, TModel> : BaseAsyncPublisherMockBuilder<TBuilder, TActor, TModel>
        where TBuilder : BaseModelActorMockBuilder<TBuilder, TActor, TModel>
        where TActor : class, IModelActor<TModel>
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
        
    public abstract class BaseModelActorMockBuilder<TBuilder, TActor, TId, TModel> : BaseModelActorMockBuilder<TBuilder, TActor, TModel> 
        where TBuilder : BaseModelActorMockBuilder<TBuilder, TActor, TId, TModel> 
        where TActor : class, IModelActor<TId, TModel>
        where TModel : IHasId<TId>
    {
          
    }
}
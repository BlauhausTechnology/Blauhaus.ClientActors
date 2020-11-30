using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Utils.Contracts;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.ClientActors.TestHelpers
{
    public abstract class BaseModelActorMockBuilder<TBuilder, TActor, TId, TModel> : BaseAsyncPublisherMockBuilder<TBuilder, TActor, TModel> 
        where TBuilder : BaseModelActorMockBuilder<TBuilder, TActor, TId, TModel> 
        where TActor : class, IModelActor<TId, TModel>
        where TModel : IHasId<TId>
    {
        

        public TBuilder Where_GetModelAsync_returns(TModel model)
        {
            Mock.Setup(x => x.GetModelAsync())
                .ReturnsAsync(model);
            return (TBuilder) this;
        }
          
    }
}
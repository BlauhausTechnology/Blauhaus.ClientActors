using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Responses;
using Blauhaus.TestHelpers.MockBuilders;
using Moq;

namespace Blauhaus.ClientActors.TestHelpers
{

    public class VirtualActorMockBuilder<TActor> : VirtualActorMockBuilder<VirtualActorMockBuilder<TActor>, TActor> 
    {

    }

    public abstract class VirtualActorMockBuilder<TBuilder, TActor> : BaseMockBuilder<TBuilder, IVirtualActor<TActor>> 
        where TBuilder : VirtualActorMockBuilder<TBuilder, TActor>
    {

        public TBuilder Where_InvokeAsync_returns<TResponse>(TResponse response)
        {
            Mock.Setup(x => x.InvokeAsync(It.IsAny<Expression<Func<TActor, Func<Task<TResponse>>>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            Mock.Setup(x => x.InvokeAsync(It.IsAny<Expression<Func<TActor, Func<TResponse>>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            return (TBuilder) this;
        }
        
        public TBuilder Where_InvokeAsync_returns<TResponse, TMessage>(TResponse response)
        {
            Mock.Setup(x => x.InvokeAsync(It.IsAny<Expression<Func<TActor, Func<TMessage, Task<TResponse>>>>>(), It.IsAny<TMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            Mock.Setup(x => x.InvokeAsync(It.IsAny<Expression<Func<TActor, Func<TMessage, TResponse>>>>(), It.IsAny<TMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            return (TBuilder) this;
        }

        public TBuilder Where_HandleAsync_returns<TResponse, TCommand>(Response<TResponse> response)
        {
            Mock.Setup(x => x.HandleAsync<TResponse, TCommand>(It.IsAny<TCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
             
            return (TBuilder) this;
        }

        public TBuilder Where_HandleVoidAsync_returns<TResponse, TCommand>(Response response)
        {
            Mock.Setup(x => x.HandleVoidAsync<TCommand>(It.IsAny<TCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
             
            return (TBuilder) this;
        }


        public void VerifyInvokeAsync(Expression<Func<TActor, Func<Task>>> handler)
        {
            Mock.Verify(x => x.InvokeAsync(handler, It.IsAny<CancellationToken>()));
        }
    }
}
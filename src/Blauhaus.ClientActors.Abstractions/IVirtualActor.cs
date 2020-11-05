using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Responses;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IVirtualActor : IAsyncDisposable
    {

    }
    public interface IVirtualActor<TActor> : IVirtualActor  
    {
        Task InvokeAsync(Expression<Func<TActor, Func<Task>>> handler, CancellationToken token = default);
        Task<TResponse> InvokeAsync<TResponse>(Expression<Func<TActor, Func<Task<TResponse>>>> handler, CancellationToken token = default);
        Task InvokeAsync<TMessage>(Expression<Func<TActor, Func<TMessage, Task>>> handler, TMessage message, CancellationToken token = default);
        Task<TResponse> InvokeAsync<TResponse, TMessage>(Expression<Func<TActor, Func<TMessage, Task<TResponse>>>> handler, TMessage message, CancellationToken token = default);

        Task InvokeAsync(Expression<Func<TActor, Action>> handler, CancellationToken token = default);
        Task<TResponse> InvokeAsync<TResponse>(Expression<Func<TActor, Func<TResponse>>> handler, CancellationToken token = default);
        Task InvokeAsync<TMessage>(Expression<Func<TActor, Action<TMessage>>> handler, TMessage message, CancellationToken token = default);
        Task<TResponse> InvokeAsync<TResponse, TMessage>(Expression<Func<TActor, Func<TMessage, TResponse>>> handler, TMessage message, CancellationToken token = default);

        Task<Response> HandleVoidAsync<TCommand>(TCommand command, CancellationToken token = default);
        Task<Response<TResponse>> HandleAsync<TResponse, TCommand>(TCommand command, CancellationToken token = default);
    }
}
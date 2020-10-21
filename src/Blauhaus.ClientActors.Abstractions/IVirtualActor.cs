using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.Responses;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IVirtualActor<TActor> : IAsyncDisposable where TActor : IClientActor
    {

        Task InvokeAsync(Expression<Func<TActor, Func<Task>>> handler);
        Task<TResponse> InvokeAsync<TResponse>(Expression<Func<TActor, Func<Task<TResponse>>>> handler);
        Task InvokeAsync<TMessage>(Expression<Func<TActor, Func<TMessage, Task>>> handler, TMessage message);
        Task<TResponse> InvokeAsync<TResponse, TMessage>(Expression<Func<TActor, Func<TMessage, Task<TResponse>>>> handler, TMessage message);

        Task InvokeAsync(Expression<Func<TActor, Action>> handler);
        Task<TResponse> InvokeAsync<TResponse>(Expression<Func<TActor, Func<TResponse>>> handler);
        Task InvokeAsync<TMessage>(Expression<Func<TActor, Action<TMessage>>> handler, TMessage message);
        Task<TResponse> InvokeAsync<TResponse, TMessage>(Expression<Func<TActor, Func<TMessage, TResponse>>> handler, TMessage message);

        Task<Response> HandleVoidAsync<TCommand>(TCommand command, CancellationToken token);
        Task<Response<TResponse>> HandleAsync<TResponse, TCommand>(TCommand command, CancellationToken token);

    }
}
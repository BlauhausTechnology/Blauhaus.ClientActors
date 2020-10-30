using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.ClientActors.StandaloneActors;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.Responses;

namespace Blauhaus.ClientActors.VirtualActors
{
    public class VirtualActor<TActor> : BaseStandaloneActor, IVirtualActor<TActor> where TActor : class, IInitializeById
    {
        private readonly TActor _actor;

        public VirtualActor(TActor actor)
        {
            _actor = actor;
        }
         
         
        public async Task InvokeAsync(Expression<Func<TActor, Func<Task>>> handler, CancellationToken token = default)
        {
            await DoAsync(async () =>
            {
                var actorFunction = handler.Compile().Invoke(_actor);
                await actorFunction.Invoke();
            }, token);
        }
        
        public async Task<TResponse> InvokeAsync<TResponse>(Expression<Func<TActor, Func<Task<TResponse>>>> handler, CancellationToken token = default)
        {
            return await DoAsync(async () =>
            {
                var actorFunction = handler.Compile().Invoke(_actor);
                return await actorFunction.Invoke();
            }, token);
        }
        
        public async Task InvokeAsync<TMessage>(Expression<Func<TActor, Func<TMessage, Task>>> handler, TMessage message, CancellationToken token = default)
        {
            await DoAsync(() =>
            { 
                var actorFunction = handler.Compile().Invoke(_actor);
                return actorFunction.Invoke(message);
            }, token);
        }
        
        public async Task<TResponse> InvokeAsync<TResponse, TMessage>(Expression<Func<TActor, Func<TMessage, Task<TResponse>>>> handler, TMessage message, CancellationToken token = default)
        {
            return await DoAsync(async () =>
            { 
                var actorFunction = handler.Compile().Invoke(_actor);
                return await actorFunction.Invoke(message);
            }, token);
        }

        public async Task InvokeAsync(Expression<Func<TActor, Action>> handler, CancellationToken token = default)
        {
            await DoAsync(() =>
            { 
                var actorFunction = handler.Compile().Invoke(_actor);
                actorFunction.Invoke();
            }, token);
        }
        
        public async Task<TResponse> InvokeAsync<TResponse>(Expression<Func<TActor, Func<TResponse>>> handler, CancellationToken token = default)
        {
            return await DoAsync(() =>
            { 
                var actorFunction = handler.Compile().Invoke(_actor);
                return actorFunction.Invoke();
            }, token);
        }
        
        public async Task InvokeAsync<TMessage>(Expression<Func<TActor, Action<TMessage>>> handler, TMessage message, CancellationToken token = default)
        {
            await DoAsync(() =>
            { 
                var actorFunction = handler.Compile().Invoke(_actor);
                actorFunction.Invoke(message);
            }, token);
        }
        
        public async Task<TResponse> InvokeAsync<TResponse, TMessage>(Expression<Func<TActor, Func<TMessage, TResponse>>> handler, TMessage message, CancellationToken token = default)
        {
            return await DoAsync(() =>
            { 
                var actorFunction = handler.Compile().Invoke(_actor);
                return actorFunction.Invoke(message);
            }, token);
        }

        public async Task<Response> HandleVoidAsync<TCommand>(TCommand command, CancellationToken token = default) where TCommand : notnull
        {
            return await DoAsync(async () =>
            { 
                // ReSharper disable once SuspiciousTypeConversion.Global
                if (!(_actor is IVoidCommandHandler<TCommand> handler))
                {
                    throw new InvalidOperationException($"{typeof(TActor).Name} must implement IVoidCommandHandler<{typeof(TCommand).Name}> to be a valid handler for {typeof(TCommand).Name}");
                }
                return  await handler.HandleAsync(command, token);
            }, token);
        }

        public async Task<Response<TResponse>> HandleAsync<TResponse, TCommand>(TCommand command, CancellationToken token = default) where TCommand : notnull
        {
            return await DoAsync(async () =>
            { 
                // ReSharper disable once SuspiciousTypeConversion.Global
                if (!(_actor is ICommandHandler<TResponse, TCommand> handler))
                {
                    throw new InvalidOperationException($"{typeof(TActor).Name} must implement ICommandHandler<{typeof(TResponse).Name}, {typeof(TCommand).Name}> to be a valid handler for {typeof(TCommand).Name}");
                }
                return  await handler.HandleAsync(command, token);
            }, token);
        }
          
    }
}
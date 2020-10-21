using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.ClientActors.StandaloneActors;
using Blauhaus.Domain.Abstractions.CommandHandlers;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.Responses;
using IClientActor = Blauhaus.ClientActors.Abstractions.IClientActor;

namespace Blauhaus.ClientActors.VirtualActors
{
    public class VirtualActor<TActor> : BaseStandaloneActor, IVirtualActor<TActor> where TActor : class, IClientActor
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly string _id;
        private readonly VirtualActorCache _virtualActorCache;
        private readonly CancellationToken _token = CancellationToken.None;

        public VirtualActor(IServiceLocator serviceLocator, string id)
        {
            _serviceLocator = serviceLocator;
            _id = id;
            _virtualActorCache = serviceLocator.Resolve<VirtualActorCache>();
        }
         
        public async Task InvokeAsync(Expression<Func<TActor, Func<Task>>> handler)
        {
            await DoAsync(async () =>
            {
                var actor = await GetActorAsync();
                var actorFunction = handler.Compile().Invoke(actor);
                await actorFunction.Invoke();
            }, _token);
        }
        
        public async Task<TResponse> InvokeAsync<TResponse>(Expression<Func<TActor, Func<Task<TResponse>>>> handler)
        {
            return await DoAsync(async () =>
            {
                var actor = await GetActorAsync();
                var actorFunction = handler.Compile().Invoke(actor);
                return await actorFunction.Invoke();
            }, _token);
        }
        
        public async Task InvokeAsync<TMessage>(Expression<Func<TActor, Func<TMessage, Task>>> handler, TMessage message)
        {
            await DoAsync(async () =>
            {
                var actor = await GetActorAsync();
                var actorFunction = handler.Compile().Invoke(actor);
                return actorFunction.Invoke(message);
            });
        }
        
        public async Task<TResponse> InvokeAsync<TResponse, TMessage>(Expression<Func<TActor, Func<TMessage, Task<TResponse>>>> handler, TMessage message)
        {
            return await DoAsync(async () =>
            {
                var actor = await GetActorAsync();
                var actorFunction = handler.Compile().Invoke(actor);
                return await actorFunction.Invoke(message);
            }, _token);
        }

        public async Task InvokeAsync(Expression<Func<TActor, Action>> handler)
        {
            await DoAsync(async () =>
            {
                var actor = await GetActorAsync();
                var actorFunction = handler.Compile().Invoke(actor);
                actorFunction.Invoke();
            }, _token);
        }
        
        public async Task<TResponse> InvokeAsync<TResponse>(Expression<Func<TActor, Func<TResponse>>> handler)
        {
            return await DoAsync(async () =>
            {
                var actor = await GetActorAsync();
                var actorFunction = handler.Compile().Invoke(actor);
                return actorFunction.Invoke();
            }, _token);
        }
        
        public async Task InvokeAsync<TMessage>(Expression<Func<TActor, Action<TMessage>>> handler, TMessage message)
        {
            await DoAsync(async () =>
            {
                var actor = await GetActorAsync();
                var actorFunction = handler.Compile().Invoke(actor);
                actorFunction.Invoke(message);
            }, _token);
        }
        
        public async Task<TResponse> InvokeAsync<TResponse, TMessage>(Expression<Func<TActor, Func<TMessage, TResponse>>> handler, TMessage message)
        {
            return await DoAsync(async () =>
            {
                var actor = await GetActorAsync();
                var actorFunction = handler.Compile().Invoke(actor);
                return actorFunction.Invoke(message);
            }, _token);
        }

        public async Task<Response> HandleVoidAsync<TCommand>(TCommand command, CancellationToken token) where TCommand : notnull
        {
            return await DoAsync(async () =>
            {
                var actor = await GetActorAsync();
                // ReSharper disable once SuspiciousTypeConversion.Global
                if (!(actor is IVoidCommandHandler<TCommand> handler))
                {
                    throw new InvalidOperationException($"{typeof(TActor).Name} must implement IVoidCommandHandler<{typeof(TCommand).Name}> to be a valid handler for {typeof(TCommand).Name}");
                }
                return  await handler.HandleAsync(command, token);
            }, _token);
        }

        public async Task<Response<TResponse>> HandleAsync<TResponse, TCommand>(TCommand command, CancellationToken token) where TCommand : notnull
        {
            return await DoAsync(async () =>
            {
                var actor = await GetActorAsync();
                // ReSharper disable once SuspiciousTypeConversion.Global
                if (!(actor is ICommandHandler<TResponse, TCommand> handler))
                {
                    throw new InvalidOperationException($"{typeof(TActor).Name} must implement ICommandHandler<{typeof(TResponse).Name}, {typeof(TCommand).Name}> to be a valid handler for {typeof(TCommand).Name}");
                }
                return  await handler.HandleAsync(command, token);
            }, _token);
        }

        private async ValueTask<TActor> GetActorAsync()
        {
            var existingActor = _virtualActorCache.Get<TActor>(_id);
            if (existingActor != null)
            {
                return existingActor;
            }

            var newActor = _serviceLocator.Resolve<TActor>();
            await newActor.InitializeAsync(_id);
            _virtualActorCache.Add(_id, newActor);
            return newActor;
        }
        
        public override async ValueTask DisposeAsync()
        {
            if (_virtualActorCache.TryRemove<TActor>(_id, out var actorToRemove))
            {
                await actorToRemove.ShutdownAsync();
            }

            await base.DisposeAsync();
        }

    }
}
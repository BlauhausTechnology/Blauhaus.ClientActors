﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Utils.Contracts;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.ClientActors.Containers
{
    public class ModelActorContainer<TActor, TId, TModel> : ActorContainer<TActor, TId>, IModelActorContainer<TActor, TId, TModel>
        where TActor : class, IModelActor<TId, TModel> 
        where TModel : IId<TId>
    {
        public ModelActorContainer(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public Task<TModel> GetModelAsync(TId id)
        {
            return InvokeAsync(async () =>
            {
                var actor = await GetActorAsync(id);
                return await actor.GetModelAsync();
            });
        }

        public Task<IReadOnlyList<TModel>> GetModelsAsync(IEnumerable<TId> actorIds)
        {
            return InvokeAsync(async () =>
            {
                var actors = await GetActorsAsync(actorIds);

                var getModelTasks = new List<Task<TModel>>();
                foreach (var modelActor in actors)
                {
                    getModelTasks.Add(modelActor.GetModelAsync());
                }
                return (IReadOnlyList<TModel>) await Task.WhenAll(getModelTasks);
            });
        }

        public Task<IDisposable> SubscribeAsync(TId id, Func<TModel, Task> handler)
        {
            return InvokeAsync(async () =>
            {
                var actor = await GetActorAsync(id);
                return await actor.SubscribeAsync(handler);
            });
        }
    }
}
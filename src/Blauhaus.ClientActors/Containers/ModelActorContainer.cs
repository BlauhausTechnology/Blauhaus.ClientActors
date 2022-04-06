﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Abstractions;
using Blauhaus.Common.Utils.Disposables;
using Blauhaus.Domain.Abstractions.Actors;
using Blauhaus.Ioc.Abstractions;
using Microsoft.Extensions.Logging;

namespace Blauhaus.ClientActors.Containers
{
    public class ModelActorContainer<TActor, TId, TModel> : BaseActorContainer<TActor, TId>, IModelActorContainer<TActor, TId, TModel>
        where TActor : class, IModelActor<TModel, TId> 
        where TModel : IHasId<TId>
    {

        private readonly List<SavedSubscription> _activeModelSubscriptions = new();
         
        public ModelActorContainer(
            IAnalyticsLogger<ModelActorContainer<TActor, TId, TModel>> logger, 
            IServiceLocator serviceLocator) 
                : base(logger, serviceLocator)
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

        public Task<IReadOnlyList<TModel>> GetActiveModelsAsync()
        {
            return InvokeAsync(async () =>
            {
                var actors = GetActiveActors();

                var getModelTasks = new List<Task<TModel>>();
                foreach (var modelActor in actors)
                {
                    getModelTasks.Add(modelActor.GetModelAsync());
                }
                
                var models = await Task.WhenAll(getModelTasks);
                
                return (IReadOnlyList<TModel>) models;
            });
        }

        public Task<IDisposable> SubscribeToModelAsync(TId id, Func<TModel, Task> handler)
        {
            return InvokeAsync(async () =>
            {
                var actor = await GetActorAsync(id);
                return await actor.SubscribeAsync(handler);
            });
        }

        public async Task<IDisposable> SubscribeToActiveModelsAsync(Func<TModel, Task> handler, Func<TModel, bool>? filter = null)
        {
            var disposables = new Disposables();

            var activeActors = GetActiveActors();
            foreach (var activeActor in activeActors)
            {
                disposables.Add(await activeActor.SubscribeAsync(handler, filter));
            }

            _activeModelSubscriptions.Add(new SavedSubscription(disposables, handler, filter));
            return disposables;

        }

        protected override async Task HandleNewActorAsync(TActor newActor)
        {
            if (_activeModelSubscriptions.Count > 0)
            {
                foreach (var modelSubscription in _activeModelSubscriptions)
                {
                    var disposables = modelSubscription.Disposables;
                    var handler = modelSubscription.Handler;
                    var filter = modelSubscription.Filter;
                    disposables.Add(await newActor.SubscribeAsync(handler, filter)); 
                }

                Logger.LogTrace("Added {SubscriptionCount} subscriptions to actor of type {ActorTypeName}", _activeModelSubscriptions.Count, typeof(TActor).Name);
            }
            
        }

        
        private class SavedSubscription
        {
            public SavedSubscription(Disposables disposables, Func<TModel, Task> handler, Func<TModel, bool>? filter)
            {
                Disposables = disposables;
                Handler = handler;
                Filter = filter;
            }

            public Disposables Disposables { get; }
            public Func<TModel, Task> Handler { get; }
            public Func<TModel, bool>? Filter { get; }
        }

    }

}
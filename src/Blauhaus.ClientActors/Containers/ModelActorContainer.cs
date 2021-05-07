using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Abstractions;
using Blauhaus.Common.Utils.Disposables;
using Blauhaus.Ioc.Abstractions;

namespace Blauhaus.ClientActors.Containers
{
    public class ModelActorContainer<TActor, TId, TModel> : ActorContainer<TActor, TId>, IModelActorContainer<TActor, TId, TModel>
        where TActor : class, IModelActor<TId, TModel> 
        where TModel : IHasId<TId>
    {

        private readonly List<Tuple<Disposables, Func<TModel, Task>>> _activeMmodelSubscriptions = new();

        public ModelActorContainer(
            IServiceLocator serviceLocator,
            IAnalyticsService analyticsService) 
                : base(serviceLocator, analyticsService)
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
                
                AnalyticsService.Debug($"Loading {models.Length} {nameof(TModel)} from actors");

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

        public async Task<IDisposable> SubscribeToActiveModelsAsync(Func<TModel, Task> handler)
        {
            var disposables = new Disposables();

            var activeActors = GetActiveActors();
            foreach (var activeActor in activeActors)
            {
                disposables.Add(await activeActor.SubscribeAsync(handler));
            }

            AnalyticsService.Debug($"Subscribed to {activeActors.Count} actors of type {nameof(TActor)}");

            _activeMmodelSubscriptions.Add(new Tuple<Disposables, Func<TModel, Task>>(disposables, handler));
            return disposables;

        }


        protected override async Task HandleNewActorAsync(TActor newActor)
        {
            if (_activeMmodelSubscriptions.Count > 0)
            {
                foreach (var modelSubscription in _activeMmodelSubscriptions)
                {
                    var disposables = modelSubscription.Item1;
                    var handler = modelSubscription.Item2;
                    disposables.Add(await newActor.SubscribeAsync(handler)); 
                }
            }
            
            AnalyticsService.Debug($"Added {_activeMmodelSubscriptions.Count} subscriptions to actor of type {nameof(TActor)}");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

                return (IReadOnlyList<TModel>) await Task.WhenAll(getModelTasks);
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
            foreach (var activeActor in GetActiveActors())
            {
                disposables.Add(await activeActor.SubscribeAsync(handler));
            }

            _activeMmodelSubscriptions.Add(new Tuple<Disposables, Func<TModel, Task>>(disposables, handler));
            return disposables;

        }


        protected override async Task HandleNewActorAsync(TActor newActor)
        {
            foreach (var modelSubscription in _activeMmodelSubscriptions)
            {
                var disposables = modelSubscription.Item1;
                var handler = modelSubscription.Item2;
                disposables.Add(await newActor.SubscribeAsync(handler));
                var model = await newActor.GetModelAsync();
                await handler.Invoke(model);
            }
        }
    }
}
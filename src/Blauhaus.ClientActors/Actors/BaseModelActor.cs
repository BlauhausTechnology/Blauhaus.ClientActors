using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Abstractions;

namespace Blauhaus.ClientActors.Actors
{
    public abstract class BaseModelActor<TId, TModel> : BaseIdActor<TId>, IModelActor<TId, TModel>
        where TModel : class, IHasId<TId>
    {
        protected TModel? Model;
        

        public Task<TModel> GetModelAsync()
        {
            return InvokeAsync(async () => await GetOrLoadModelAsync());
        }

        public override Task ReloadAsync()
        {
            return InvokeAsync(async () =>
            {
                await ReloadSelfAsync();
            });
        }
         
        public Task<IDisposable> SubscribeAsync(Func<TModel, Task> handler)
        {
            return InvokeAsync(async () => await AddSubscribeAsync(handler, GetOrLoadModelAsync));
        }

        private async Task<TModel> GetOrLoadModelAsync()
        {
            return Model ??= await LoadModelAsync();
        }

        
        protected async Task<TModel> ReloadSelfAsync()
        {
            Model = await LoadModelAsync();
            await UpdateSubscribersAsync(Model);
            return Model;
        }

        protected Task UpdateModelAsync(TModel model)
        {
            Model = model;
            return UpdateSubscribersAsync(Model);
        }

        protected abstract Task<TModel> LoadModelAsync();
    }
}
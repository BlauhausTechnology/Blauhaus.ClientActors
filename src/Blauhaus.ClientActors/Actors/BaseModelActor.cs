using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Abstractions;

namespace Blauhaus.ClientActors.Actors
{
    public abstract class BaseModelActor<TId, TModel> : BaseIdActor<TId>, IModelActor<TId, TModel>
        where TModel : class, IHasId<TId>
    {
        private TModel? _model;
        

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
         
        public Task<IDisposable> SubscribeAsync(Func<TModel, Task> handler, Func<TModel, bool>? filter = null)
        {
            return InvokeAsync(async () => await SubscribeAsync(handler, GetOrLoadModelAsync, filter));
        }

        private async Task<TModel> GetOrLoadModelAsync()
        {
            return _model ??= await LoadModelAsync();
        }

        
        protected async Task<TModel> ReloadSelfAsync()
        {
            _model = await LoadModelAsync();
            await UpdateSubscribersAsync(_model);
            return _model;
        }


        protected abstract Task<TModel> LoadModelAsync();
    }
}
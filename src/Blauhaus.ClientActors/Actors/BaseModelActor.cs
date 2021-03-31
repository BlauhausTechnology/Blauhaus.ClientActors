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
            return InvokeInterleavedAsync(async () => await GetOrLoadModelAsync());
        }

        public override Task ReloadAsync()
        {
            return InvokeInterleavedAsync(async () =>
            {
                await ReloadSelfAsync();
            });
        }
        public Task<IDisposable> SubscribeAsync(Func<TModel, Task> handler, Func<TModel, bool>? filter = null)
        {
            return Task.FromResult(AddSubscriber(handler, filter));
        }
         
        protected async Task<TModel> GetOrLoadModelAsync()
        {
            return _model ??= await LoadModelAsync();
        }
        
        protected async Task<TModel> ReloadSelfAsync()
        {
            _model = await LoadModelAsync();
            await UpdateSubscribersAsync(_model);
            return _model;
        }

        protected async Task<TModel> UpdateModelAsync(Func<TModel, TModel> modelUpdater)
        {
            var model = await GetOrLoadModelAsync();
            
            _model = modelUpdater.Invoke(model);
            await UpdateSubscribersAsync(_model);
            return _model;
        }

        protected abstract Task<TModel> LoadModelAsync();

    }
}
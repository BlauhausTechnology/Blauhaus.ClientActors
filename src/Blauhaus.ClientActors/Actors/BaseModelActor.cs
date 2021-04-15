using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Abstractions;

namespace Blauhaus.ClientActors.Actors
{

    public abstract class BaseModelActor<TModel> : BaseActor, IModelActor<TModel>
    {
        protected TModel? Model;
        
        public Task<IDisposable> SubscribeAsync(Func<TModel, Task> handler, Func<TModel, bool>? filter = null)
        {
            return Task.FromResult(AddSubscriber(handler, filter));
        }
        
        public Task<TModel> GetModelAsync()
        {
            return InvokeAsync(async () => await GetOrLoadModelAsync());
        }
        
        public Task ReloadAsync()
        {
            return InvokeAsync(async () =>
            {
                await ReloadSelfAsync();
            });
        }
        
        protected async Task<TModel> GetOrLoadModelAsync()
        {
            return Model ??= await LoadModelAsync();
        }
        
        protected async Task<TModel> ReloadSelfAsync()
        {
            Model = await LoadModelAsync();
            await UpdateSubscribersAsync(Model);
            return Model;
        }

        protected async Task<TModel> UpdateModelAsync(Func<TModel, TModel> modelUpdater)
        {
            var model = await GetOrLoadModelAsync();
            
            Model = modelUpdater.Invoke(model);
            await UpdateSubscribersAsync(Model);
            return Model;
        }
         
        protected abstract Task<TModel> LoadModelAsync();


    }
    
    public abstract class BaseModelActor<TId, TModel> : BaseModelActor<TModel>, IModelActor<TId, TModel>
        where TModel : class, IHasId<TId>
    {

        private TId? _id;
        public TId Id
        {
            get
            {
                if (_id == null)
                    throw new InvalidOperationException("Actor has not been initialized with an Id");
                return _id;
            }
        }

        public Task InitializeAsync(TId id)
        {
            _id = id;
            return OnInitializedAsync(id);
        }

        protected virtual Task OnInitializedAsync(TId id)
        {
            return Task.CompletedTask;
        }

        


    }
}
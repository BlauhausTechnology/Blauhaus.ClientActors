using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Abstractions;
using Blauhaus.Domain.Abstractions.Actors;

namespace Blauhaus.ClientActors.Actors
{
     
    public abstract class BaseModelActor<TModel, TId> : BaseActor<TModel>, IModelActor<TModel, TId>
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

        protected TModel? Model;

        protected override async Task<TModel> GetAsync()
        {
            return Model ??= await LoadModelAsync();
        }

        public Task<TModel> GetModelAsync()
        {
            return InvokeAsync(async () => await GetAsync());
        }
        
        public Task ReloadAsync()
        {
            return InvokeAsync(async () =>
            {
                await ReloadSelfAsync();
            });
        }
         
        
        protected async Task<TModel> ReloadSelfAsync()
        {
            Model = await LoadModelAsync();
            await UpdateSubscribersAsync(Model);
            return Model;
        }

        protected async Task<TModel> UpdateModelAsync(Func<TModel, TModel> modelUpdater)
        {
            var model = await GetAsync();
            
            Model = modelUpdater.Invoke(model);
            await UpdateSubscribersAsync(Model);
            return Model;
        }

        protected async Task<TModel> SetModelAsync(TModel newModel)
        {
            Model = newModel;
            await UpdateSubscribersAsync(Model);
            return Model;
        }
         
        protected abstract Task<TModel> LoadModelAsync();



    }
}
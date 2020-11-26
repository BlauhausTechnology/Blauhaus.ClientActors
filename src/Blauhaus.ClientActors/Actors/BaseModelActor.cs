using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Utils.Contracts;
using Blauhaus.Common.Utils.Disposables;

namespace Blauhaus.ClientActors.Actors
{
    public abstract class BaseModelActor<TId, TModel> : BaseIdActor<TId>, IModelActor<TId, TModel>
        where TModel : class, IId<TId>
    {
        private TModel? _model;
        
        
        private readonly Dictionary<Type, List<Func<object, Task>>> _subscribers = new Dictionary<Type, List<Func<object, Task>>>();

        public Task<TModel> GetModelAsync()
        {
            return InvokeAsync(async () => await GetOrLoadModelAsync());
        }

        public override Task ReloadAsync()
        {
            return InvokeAsync(async () =>
            {
                _model = await LoadModelAsync();
                await UpdateSubscribersAsync(_model);
            });
        }
         
        public Task<IDisposable> SubscribeAsync(Func<TModel, Task> handler)
        {
            return InvokeAsync(async () => await SubscribeAsync<TModel>(handler, GetOrLoadModelAsync));
        }

        private async Task<TModel> GetOrLoadModelAsync()
        {
            return _model ??= await LoadModelAsync();
        }

        protected abstract Task<TModel> LoadModelAsync();
    }
}
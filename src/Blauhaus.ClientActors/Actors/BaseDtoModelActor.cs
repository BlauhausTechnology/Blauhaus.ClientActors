using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Abstractions;
using Blauhaus.Common.Utils.Disposables;
using Blauhaus.Domain.Abstractions.DtoCaches;

namespace Blauhaus.ClientActors.Actors
{
    public abstract class BaseDtoModelActor<TModel, TDto, TDtoLoader, TId> : BasePublisher, IDtoModelActor<TModel, TId>
        where TModel : class, IHasId<TId>
        where TDtoLoader : IDtoLoader<TDto, TId>
        where TDto : class, IHasId<TId>
        where TId : IEquatable<TId>
    {
        protected readonly TDtoLoader DtoLoader;
        protected TId Id = default!;
        private IDisposable? _dtoCacheToken;
        private TModel? _model;

        protected BaseDtoModelActor(TDtoLoader dtoLoader)
        {
            DtoLoader = dtoLoader;
        }

        public async Task InitializeAsync(TId id)
        {
            Id = id;

            _dtoCacheToken = await DtoLoader.SubscribeAsync(async dto =>
            {
                _model = await ConstructModelAsync(dto);
                await UpdateSubscribersAsync(_model);
            }, x => x.Id.Equals(Id)); 
        }
        
        protected virtual async Task<TModel> LoadModelAsync()
        {
            var dto = await DtoLoader.GetOneAsync(Id);
            return await ConstructModelAsync(dto);
        }
        
        public Task<IDisposable> SubscribeAsync(Func<TModel, Task> handler, Func<TModel, bool>? filter = null)
        {
            return Task.FromResult(AddSubscriber(handler, filter));
        }

        public async Task ReloadAsync()
        {
            _model = await LoadModelAsync();
            await UpdateSubscribersAsync(_model);
        }

        public async Task<TModel> GetModelAsync()
        {
            return _model ??= await LoadModelAsync();
        }

        protected abstract Task<TModel> ConstructModelAsync(TDto dto);

        public void Dispose()
        {
            _dtoCacheToken?.Dispose();
        }
    }
}
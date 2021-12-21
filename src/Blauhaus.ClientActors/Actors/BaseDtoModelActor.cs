using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Abstractions;
using Blauhaus.Common.Utils.Disposables;
using Blauhaus.Domain.Abstractions.Actors;
using Blauhaus.Domain.Abstractions.DtoCaches;

namespace Blauhaus.ClientActors.Actors
{
    public abstract class BaseDtoModelActor<TModel, TDto, TDtoLoader, TId> : BaseModelActor<TModel, TId>, IDtoModelActor<TModel, TDto, TId>
        where TModel : class, IHasId<TId>
        where TDtoLoader : IDtoLoader<TDto, TId>
        where TDto : class, IHasId<TId>
        where TId : IEquatable<TId>
    {
        protected readonly TDtoLoader DtoLoader;
        protected TDto Dto = null!;
        private IDisposable? _dtoCacheToken;
        private TModel? _model;

        protected BaseDtoModelActor(TDtoLoader dtoLoader)
        {
            DtoLoader = dtoLoader;
        }

        protected override async Task OnInitializedAsync(TId id)
        {
            await base.OnInitializedAsync(id);
            Dto = await DtoLoader.GetOneAsync(Id);
        }

        public async Task SubscribeToDtoAsync()
        {
            _dtoCacheToken = await DtoLoader.SubscribeAsync(async dto =>
            {
                Dto = dto;
                _model = await ConstructModelAsync(dto);
                await UpdateSubscribersAsync(_model);
            }, x => x.Id.Equals(Id));

        }

        public virtual Task<TDto> GetDtoAsync()
        {
            return Task.FromResult(Dto);
        }

        protected override async Task<TModel> LoadModelAsync()
        {
            var dto = await DtoLoader.GetOneAsync(Id);
            return await ConstructModelAsync(dto);
        }

        protected abstract Task<TModel> ConstructModelAsync(TDto dto);
         
        public override ValueTask DisposeAsync()
        {
            _dtoCacheToken?.Dispose();
            return base.DisposeAsync();
        }

    }
}
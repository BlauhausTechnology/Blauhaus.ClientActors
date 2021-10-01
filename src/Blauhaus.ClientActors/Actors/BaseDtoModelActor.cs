using System;
using System.Threading.Tasks;
using Blauhaus.Common.Abstractions;
using Blauhaus.Domain.Abstractions.DtoCaches;

namespace Blauhaus.ClientActors.Actors
{
    public abstract class BaseDtoModelActor<TId, TModel, TDto, TDtoCache> : BaseModelActor<TId, TModel>
        where TModel : class, IHasId<TId>
        where TDtoCache : IDtoCache<TDto, TId>
        where TDto : class, IHasId<TId>
        where TId : IEquatable<TId>
    {
        protected readonly TDtoCache DtoCache;
        private IDisposable? _dtoCacheToken;

        protected BaseDtoModelActor(TDtoCache dtoCache)
        {
            DtoCache = dtoCache;
        }
        
        protected override async Task OnInitializedAsync(TId id)
        {
            await base.OnInitializedAsync(id);

            _dtoCacheToken = await DtoCache.SubscribeAsync(async dto =>
            {
                await ReloadSelfAsync();
            }, x => x.Id.Equals(id)); 
        }
        
        protected override async Task<TModel> LoadModelAsync()
        {
            var dto = await DtoCache.GetOneAsync(Id);
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
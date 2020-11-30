using System;
using System.Threading.Tasks;
using Blauhaus.Common.Utils.Contracts;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IModelActor<in TId, TModel> : IActor<TId> , IPublish<TModel>
        where TModel : IId<TId>
    {

        Task<TModel> GetModelAsync();
    }
}
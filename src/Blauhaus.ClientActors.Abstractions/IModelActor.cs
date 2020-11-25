using System;
using System.Threading.Tasks;
using Blauhaus.Common.Utils.Contracts;

namespace Blauhaus.ClientActors.Abstractions
{
    public interface IModelActor<in TId, TModel> : IActor<TId> 
        where TModel : IId<TId>
    {

        Task<TModel> GetModelAsync();
        Task<IDisposable> SubscribeAsync(Func<TModel, Task> handler);
    }
}
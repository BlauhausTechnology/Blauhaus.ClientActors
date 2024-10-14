using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;

namespace Blauhaus.ClientActors.Actors
{
    public abstract class BaseIdActor<TId> : BaseActor, IActor<TId>
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

        public abstract Task ReloadAsync();

    }
}
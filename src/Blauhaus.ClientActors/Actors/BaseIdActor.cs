using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Utils.Contracts;

namespace Blauhaus.ClientActors.Actors
{
    public abstract class BaseIdActor<TId> : BaseActor, IActor<TId>
    {
        private TId _id;
        protected TId Id
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
            return Task.CompletedTask;
        }

        public abstract Task ReloadAsync();

    }
}
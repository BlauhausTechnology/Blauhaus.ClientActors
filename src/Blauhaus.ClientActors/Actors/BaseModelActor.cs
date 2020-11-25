﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.Common.Utils.Contracts;
using Blauhaus.Common.Utils.Disposables;

namespace Blauhaus.ClientActors.Actors
{
    public abstract class BaseModelActor<TId, TModel> : BaseIdActor<TId>, IModelActor<TId, TModel>
        where TModel : IId<TId>
    {
        private TModel? _model;
        
        private readonly List<Func<TModel, Task>> _subscribers = new List<Func<TModel, Task>>();

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
            return InvokeAsync(() =>
            {
                _subscribers.Add(handler);
                return (IDisposable) new ActionDisposable(() => _subscribers.Remove(handler));
            });
        }

        private async Task<TModel> GetOrLoadModelAsync()
        {
            if (_model == null)
            {
                _model = await LoadModelAsync();
            }

            return _model;
        }

        private Task UpdateSubscribersAsync(TModel model)
        {
            if (_subscribers.Count > 0)
            {
                var tasks = _subscribers.Select(handler => handler.Invoke(model)).ToList();
                return Task.WhenAll(tasks);
            }

            return Task.CompletedTask;
        }

        protected abstract Task<TModel> LoadModelAsync();
    }
}
using System;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.ClientActors.Containers;
using Blauhaus.Common.Utils.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blauhaus.ClientActors._Ioc
{
    public static class ServiceCollectionExtensions
    { 

        public static IServiceCollection AddActor<TActor, TActorImplementation, TId>(this IServiceCollection services)
            where TActor : class, IActor<TId> 
            where TActorImplementation : class, TActor
        {
            services.TryAddSingleton<IActorContainer<TActor, TId>, ActorContainer<TActor, TId>>();
            services.AddTransient<TActor, TActorImplementation>();  
            return services;
        }
        
        public static IServiceCollection AddModelActor<TActor, TActorImplementation, TId, TModel>(this IServiceCollection services)
            where TActor : class, IModelActor<TId, TModel> 
            where TActorImplementation : class, TActor
            where TModel : IId<TId>
        {
            services.TryAddSingleton<IModelActorContainer<TActor, TId, TModel>, ModelActorContainer<TActor, TId, TModel>>();
            services.AddTransient<TActor, TActorImplementation>();  
            return services;
        }

         
    }
}
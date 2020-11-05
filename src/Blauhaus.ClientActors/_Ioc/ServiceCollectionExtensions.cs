using Blauhaus.ClientActors.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blauhaus.ClientActors._Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddActor<TActor>(this IServiceCollection services)
            where TActor : class, IActor  
        {

            services.TryAddSingleton<IActorContainer<TActor>, ActorContainer<TActor>>();
            
            //must be transient because container manages lifecycle
            services.AddTransient<TActor>();  

            return services;
        }


        public static IServiceCollection AddActor<TActor, TActorImplementation>(this IServiceCollection services)
            where TActor : class, IActor 
            where TActorImplementation : class, TActor
        {

            services.TryAddSingleton<IActorContainer<TActor>, ActorContainer<TActor>>();
            
            //must be transient because container manages lifecycle
            services.AddTransient<TActor, TActorImplementation>();  

            return services;
        }
         
    }
}
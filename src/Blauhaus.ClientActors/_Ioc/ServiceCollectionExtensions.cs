using Blauhaus.ClientActors.Abstractions;
using Blauhaus.ClientActors.Manager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blauhaus.ClientActors._Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddActorContainer<TActor>(this IServiceCollection services) where TActor : class, IInitializeById
        {
            services.TryAddSingleton<IActorContainer<TActor>, ActorContainer<TActor>>();
            services.AddTransient<TActor>();

            return services;
        }
         
    }
}
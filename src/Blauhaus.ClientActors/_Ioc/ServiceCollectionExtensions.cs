using Blauhaus.ClientActors.Abstractions;
using Blauhaus.ClientActors.VirtualActors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using IVirtualActorFactory = Blauhaus.ClientActors.Abstractions.IVirtualActorFactory;

namespace Blauhaus.ClientActors._Ioc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddActor<TActor>(this IServiceCollection services) where TActor : class
        {
            services.TryAddSingleton<IVirtualActorFactory, VirtualActorFactory>();
            services.AddTransient<TActor>();

            return services;
        }
         
    }
}
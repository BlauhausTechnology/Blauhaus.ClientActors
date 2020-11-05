using System;
using System.Threading;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.ClientActors.Tests._Base;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.ClientActors.VirtualActors;
using Blauhaus.Ioc.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.ClientActors.Tests.VirtualActorTests._Base
{
    public class BaseVirtualActorTest<TActor> : BaseActorTest<VirtualActor<TActor>> 
        where TActor : class, IInitializeById
    {
        protected string ActorId;
        protected IServiceLocator ServiceLocator;

        public override void Setup()
        {
            base.Setup();
            
            Services.AddSingleton<TestVirtualActor>();
            Services.AddSingleton<ExceptionActor>();
            ActorId = Guid.NewGuid().ToString();
        }
         

        protected override VirtualActor<TActor> ConstructSut()
        {
            ServiceLocator = Services.BuildServiceProvider().GetRequiredService<IServiceLocator>();
            var virtualActor = new VirtualActor<TActor>(ServiceLocator.Resolve<TActor>());
            Task.Run(async () => await virtualActor.InvokeAsync(x => x.InitializeAsync, ActorId, CancellationToken.None));
            return virtualActor;
        }

    }
}
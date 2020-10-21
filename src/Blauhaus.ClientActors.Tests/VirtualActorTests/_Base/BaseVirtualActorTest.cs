using System;
using Blauhaus.ClientActors.Abstractions;
using Blauhaus.ClientActors.Tests._Base;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.ClientActors.VirtualActors;
using Blauhaus.Ioc.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Blauhaus.ClientActors.Tests.VirtualActorTests._Base
{
    public class BaseVirtualActorTest<TActor> : BaseActorTest<VirtualActor<TActor>> 
        where TActor : class, IClientActor
    {
        protected string ActorId;
        protected IServiceLocator ServiceLocator;

        public override void Setup()
        {
            base.Setup();
            
            Services.AddSingleton<TestActor>();
            Services.AddSingleton<ExceptionActor>();
            Services.AddSingleton<VirtualActorCache>();
            ActorId = Guid.NewGuid().ToString();
        }
         

        protected override VirtualActor<TActor> ConstructSut()
        {
            ServiceLocator = Services.BuildServiceProvider().GetRequiredService<IServiceLocator>();
            return new VirtualActor<TActor>(ServiceLocator, ActorId);
        }

    }
}
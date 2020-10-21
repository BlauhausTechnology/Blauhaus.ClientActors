using Blauhaus.Ioc.Abstractions;
using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.TestHelpers.BaseTests;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests._Base
{
    public abstract class BaseActorTest<TSut> : BaseServiceTest<TSut> where TSut : class
    {
        [SetUp]
        public virtual void Setup()
        {
            base.Cleanup();

            Services.AddSingleton<IServiceLocator, DotNetCoreServiceLocator>();
        }

    }
}
using Blauhaus.Analytics.Abstractions;
using Blauhaus.Analytics.Abstractions.Service;
using Blauhaus.Analytics.TestHelpers.MockBuilders;
using Blauhaus.Ioc.Abstractions;
using Blauhaus.Ioc.DotNetCoreIocService;
using Blauhaus.TestHelpers.BaseTests;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Blauhaus.ClientActors.Tests.Base
{
    public abstract class BaseActorTest<TSut> : BaseServiceTest<TSut> where TSut : class
    {
        [SetUp]
        public virtual void Setup()
        {
            base.Cleanup();

            Services.AddSingleton<IServiceLocator, DotNetCoreServiceLocator>();

            AddService(x => MockLogger.Object);
        }

        protected AnalyticsLoggerMockBuilder<TSut> MockLogger => AddMock<AnalyticsLoggerMockBuilder<TSut>, IAnalyticsLogger<TSut>>().Invoke();
    }
}
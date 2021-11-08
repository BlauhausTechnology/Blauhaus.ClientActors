using Blauhaus.ClientActors.Tests.Base;
using Blauhaus.ClientActors.Tests.Suts;
using Blauhaus.TestHelpers.MockBuilders;
using Microsoft.Extensions.DependencyInjection;
using System;
using Moq;
using Blauhaus.ClientActors.Tests.Mocks;

namespace Blauhaus.ClientActors.Tests.Actors.DtoModelActorTests.Base
{
    public class BaseDtoModelActorTest : BaseActorTest<TestDtoModelActor>
    {
        protected TestDtoLoaderMockBuilder MockDtoLoader = null!;
        protected  Guid Id;
        protected  TestDto Dto = null!;

        public override void Setup()
        {
            base.Setup();
            
            Id = Guid.NewGuid();

            Dto = new TestDto{ Id = Id };
            MockDtoLoader = new TestDtoLoaderMockBuilder();
            MockDtoLoader.Mock.Setup(x => x.GetOneAsync(Id)).ReturnsAsync(Dto);
            Services.AddSingleton<ITestDtoLoader>(MockDtoLoader.Object);
        }

    }
}
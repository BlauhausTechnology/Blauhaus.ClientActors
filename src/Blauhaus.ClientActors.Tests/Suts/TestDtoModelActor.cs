using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Actors;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public class TestDtoModelActor : BaseDtoModelActor<ITestModel, TestDto, ITestDtoLoader, Guid>
    {
        public TestDtoModelActor(ITestDtoLoader dtoLoader) : base(dtoLoader)
        {
        }

        protected override Task<ITestModel> ConstructModelAsync(TestDto dto)
        {
            return Task.FromResult<ITestModel>(new TestModel(dto.Id, dto.RandomThing));
        }

        public async Task UpdateSubscribersWithCurrentModelAsync()
        {
            await UpdateSubscribersAsync(await GetModelAsync());
        }
         
    }
}
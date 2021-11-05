using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Actors;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public class TestModelActor : BaseModelActor<ITestModel, Guid>
    {
        protected override Task<ITestModel> LoadModelAsync()
        {
            return Task.FromResult<ITestModel>(new TestModel(Id));
        }

        public async Task UpdateSubscribersWithCurrentModelAsync()
        {
            await UpdateModelAsync(x => x);
        }
    }
}
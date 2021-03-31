using System;
using System.Threading.Tasks;
using Blauhaus.ClientActors.Actors;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public class TestModelActor : BaseModelActor<Guid, ITestModel>
    {
        protected override Task<ITestModel> LoadModelAsync()
        {
            return Task.FromResult<ITestModel>(new TestModel(Id));
        }
    }
}
using System;
using Blauhaus.Domain.Abstractions.Actors;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public interface ITestModelActor : IModelActor<ITestModel, Guid>
    {
        
    }
}
using System;
using Blauhaus.Common.Abstractions;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public interface ITestModel : IHasId<Guid>
    {
        string RandomThing { get; }
    }
}
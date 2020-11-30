using System;
using Blauhaus.Common.Utils.Contracts;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public interface ITestModel : IHasId<Guid>
    {
        string RandomThing { get; }
    }
}
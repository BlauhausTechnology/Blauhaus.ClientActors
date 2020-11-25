using System;
using Blauhaus.Common.Utils.Contracts;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public interface ITestModel : IId<Guid>
    {
        string RandomThing { get; }
    }
}
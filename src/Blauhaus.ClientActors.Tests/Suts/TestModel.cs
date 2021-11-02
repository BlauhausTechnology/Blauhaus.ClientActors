using System;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public class TestModel : ITestModel
    {
        public TestModel(Guid id, string? randomThing = null)
        {
            Id = id;
            RandomThing = randomThing ?? Guid.NewGuid().ToString();
        }

        public Guid Id { get; }
        public string RandomThing { get; }
    }
}
using System;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public class TestModel : ITestModel
    {
        public TestModel(Guid id)
        {
            Id = id;
            RandomThing = Guid.NewGuid().ToString();
        }

        public Guid Id { get; }
        public string RandomThing { get; }
    }
}
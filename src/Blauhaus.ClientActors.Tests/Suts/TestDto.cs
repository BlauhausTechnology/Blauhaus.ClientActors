using System;
using Blauhaus.Domain.Abstractions.Entities;

namespace Blauhaus.ClientActors.Tests.Suts
{
    public class TestDto : IClientEntity<Guid>
    {
        public TestDto()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public EntityState EntityState { get; set; }
        public string? RandomThing { get; set; }
        public long ModifiedAtTicks { get; set; }
    }
}
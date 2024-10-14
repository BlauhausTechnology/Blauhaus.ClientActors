using System;
using Blauhaus.Domain.Abstractions.Entities;

namespace Blauhaus.ClientActors.Abstractions;

public interface IClientEntity<out TId> : IEntity<TId>
{
    long ModifiedAtTicks { get; }
}
    
public interface IClientEntity : IClientEntity<Guid>
{
}
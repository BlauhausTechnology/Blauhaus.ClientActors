using Blauhaus.Common.Abstractions;
using System;

namespace Blauhaus.Domain.Abstractions.Entities
{
    public interface IEntity<out TId> : IHasId<TId>
    {
        EntityState EntityState { get; }
    }
    
    public interface IEntity: IEntity<Guid>
    {
        
    }
    
}

public enum EntityState
{
    Active,
    Draft,
    Archived,
    Deleted
}
namespace Edc.Core.SharedContext.Entities;

public abstract class Entity {
    protected Entity() => Id = Guid.NewGuid();
    public Guid Id { get; }
}
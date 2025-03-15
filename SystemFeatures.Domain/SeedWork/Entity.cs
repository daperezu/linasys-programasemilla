using MediatR;

namespace LinaSys.SystemFeatures.Domain.SeedWork;

public abstract class Entity
{
    private readonly List<INotification> _domainEvents = [];

    private int? _requestedHashCode;

    public IReadOnlyCollection<INotification>? DomainEvents => _domainEvents.AsReadOnly();

    public virtual long Id { get; protected set; }

    public static bool operator !=(Entity left, Entity right) => !(left == right);

    public static bool operator ==(Entity left, Entity right) => left?.Equals(right) ?? Equals(right, null);

    public void AddDomainEvent(INotification eventItem) => _domainEvents.Add(eventItem);

    public void ClearDomainEvents() => _domainEvents?.Clear();

    public override bool Equals(object? obj)
    {
        if (obj is not Entity item)
        {
            return false;
        }

        if (ReferenceEquals(this, item))
        {
            return true;
        }

        if (GetType() != item.GetType())
        {
            return false;
        }

        if (item.IsTransient() || IsTransient())
        {
            return false;
        }

        return item.Id == Id;
    }

    public override int GetHashCode()
    {
        if (IsTransient())
        {
            return base.GetHashCode();
        }

        _requestedHashCode ??= Id.GetHashCode() ^ 31;

        return _requestedHashCode.Value;
    }

    public bool IsTransient() => Id == 0;

    public void RemoveDomainEvent(INotification eventItem) => _domainEvents?.Remove(eventItem);
}

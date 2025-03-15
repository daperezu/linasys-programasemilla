namespace LinaSys.SystemFeatures.Domain.SeedWork;

public interface IRepository<T>
    where T : class, IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}

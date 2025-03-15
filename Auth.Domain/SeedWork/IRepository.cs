namespace LinaSys.Auth.Domain.SeedWork;

public interface IRepository<T>
    where T : class, IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}

using LinaSys.SystemFeatures.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace LinaSys.SystemFeatures.Infrastructure.Persistence.Repositories;

public abstract class AbstractRepository<T>(SystemFeaturesDbContext context)
    : IRepository<T>
    where T : class, IAggregateRoot
{
    public IUnitOfWork UnitOfWork => context;

    public T Add(T aggregate)
    {
        return context.Set<T>().Add(aggregate).Entity;
    }

    public void Update(T aggregate)
    {
        context.Entry(aggregate).State = EntityState.Modified;
    }
}

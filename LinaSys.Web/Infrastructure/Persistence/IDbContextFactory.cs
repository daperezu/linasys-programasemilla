using LinaSys.Shared;

namespace LinaSys.Web.Infrastructure.Persistence;

public interface IDbContextFactory
{
    IDbContext GetDbContextForRequest<TRequest>();
}

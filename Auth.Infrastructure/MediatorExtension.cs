using LinaSys.Auth.Domain.SeedWork;
using LinaSys.Auth.Infrastructure.Persistence;
using MediatR;

namespace LinaSys.Auth.Infrastructure;

internal static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, AuthDbContext context)
    {
        var domainEntities = context.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Count != 0)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEntities.SelectMany(x => x.Entity.DomainEvents!))
        {
            await mediator.Publish(domainEvent);
        }
    }
}

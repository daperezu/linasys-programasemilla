using LinaSys.SystemFeatures.Domain.SeedWork;
using LinaSys.SystemFeatures.Infrastructure.Persistence;
using MediatR;

namespace LinaSys.SystemFeatures.Infrastructure;

internal static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, SystemFeaturesDbContext context)
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

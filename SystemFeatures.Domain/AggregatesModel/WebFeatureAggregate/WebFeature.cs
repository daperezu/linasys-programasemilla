using LinaSys.SystemFeatures.Domain.SeedWork;

namespace LinaSys.SystemFeatures.Domain.AggregatesModel.WebFeatureAggregate;

public partial class WebFeature
    : Entity, IAggregateRoot
{
    public Guid ExternalId { get; set; }

    public string Name { get; set; }

    public string Area { get; set; }

    public string Controller { get; set; }

    public string Action { get; set; }

    public long? ParentId { get; set; }

    public bool IsMenu { get; set; }

    public int MenuOrder { get; set; }

    public bool IsPublic { get; set; }

    public virtual ICollection<WebFeature> InverseParent { get; set; } = new List<WebFeature>();

    public virtual WebFeature Parent { get; set; }
}

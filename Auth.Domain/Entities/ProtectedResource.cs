namespace LinaSys.Auth.Domain.Entities;

public class ProtectedResource
{
    public long Id { get; set; }

    public Guid ExternalId { get; set; }

    public int ResourceType { get; set; }

    public string Name { get; set; }

    public virtual ICollection<RoleProtectedResourcePermission> RoleProtectedResourcePermissions { get; set; } = new List<RoleProtectedResourcePermission>();

    public virtual ICollection<UserProtectedResourcePermission> UserProtectedResourcePermissions { get; set; } = new List<UserProtectedResourcePermission>();
}

namespace LinaSys.Auth.Domain.Entities;

public class RoleProtectedResourcePermission
{
    public long Id { get; set; }

    public string RoleId { get; set; }

    public long ProtectedResourceId { get; set; }

    public virtual ProtectedResource ProtectedResource { get; set; }
}

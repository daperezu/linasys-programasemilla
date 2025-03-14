namespace LinaSys.Auth.Domain.Entities;

public class UserProtectedResourcePermission
{
    public long Id { get; set; }

    public string UserId { get; set; }

    public long ProtectedResourceId { get; set; }

    public virtual ProtectedResource ProtectedResource { get; set; }
}

namespace LinaSys.Auth.Domain.Entities;

public class UserProfile
{
    public long Id { get; set; }

    public string UserId { get; set; }

    public string FullName { get; set; }

    public DateTime? CreatedAt { get; set; }
}

namespace LinaSys.Auth.Application.Queries;

public class ProtectedResourceDto
{
    public int ResourceType { get; set; }

    public Guid ExternalId { get; set; }

    public long InternalId { get; set; }

    public string Name { get; set; }
}

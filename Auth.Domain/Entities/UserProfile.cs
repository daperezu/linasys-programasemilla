using System;

namespace LinaSys.Auth.Domain.Entities;

/// <summary>
/// Stores additional user information without modifying Identity tables.
/// </summary>
public class UserProfile
{
    public long Id { get; set; } // Uses BIGINT instead of UNIQUEIDENTIFIER

    public string UserId { get; set; } = default!;

    public string FullName { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

using Microsoft.AspNetCore.Identity;

namespace LinaSys.Auth.Domain.Entities;

/// <summary>
/// The user entity, where UserName is now the Person Identification Number (PIN).
/// </summary>
public class User : IdentityUser
{
    public override string? UserName { get; set; } = default!; // Stores the PIN
}

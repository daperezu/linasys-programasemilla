using Microsoft.AspNetCore.Identity;

var passwordHasher = new PasswordHasher<string>();
string hashedPassword = passwordHasher.HashPassword("admin@linasys.com", "linasys123");
Console.WriteLine($"Hashed Password: {hashedPassword}");

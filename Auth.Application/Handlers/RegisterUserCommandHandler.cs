using LinaSys.Auth.Application.Commands;
using LinaSys.Auth.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LinaSys.Auth.Application.Handlers;

public class RegisterUserCommandHandler(UserManager<User> userManager, ILogger<RegisterUserCommandHandler> logger) : IRequestHandler<RegisterUserCommand, (User? User, IEnumerable<string>? Errors)>
{
    public async Task<(User? User, IEnumerable<string>? Errors)> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = request.IdentificationNumber,
            Email = request.Email,
            EmailConfirmed = false,
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            return (user, null);
        }

        logger.LogError("User registration failed for {Email}", request.Email);
        return (null, result.Errors.Select(s => s.Description));
    }
}

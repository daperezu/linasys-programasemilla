using LinaSys.Auth.Domain.Entities;
using MediatR;

namespace LinaSys.Auth.Application.Commands;

public record RegisterUserCommand(string IdentificationNumber, string Email, string Password) : IRequest<(User? User, IEnumerable<string>? Errors)>;

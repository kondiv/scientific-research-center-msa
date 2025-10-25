using MediatR;
using Shared.ResultPattern;

namespace AuthenticationService.App.Features.Register;

internal sealed record RegisterUserCommand(string FullName, string Email, string Login,
    string Password, string NormalizedRoleName) : IRequest<Result<Guid>>;
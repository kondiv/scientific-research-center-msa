using AuthenticationService.App.Common.Security.Tokens;
using AuthenticationService.App.Features.Dto;
using MediatR;
using Shared.ResultPattern;

namespace AuthenticationService.App.Features.Login;

internal sealed record LoginUserCommand(string Login, string Password) : IRequest<Result<JwtTokenPair>>;
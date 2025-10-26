using AuthenticationService.App.Features.Dto;
using MediatR;
using Shared.ResultPattern;

namespace AuthenticationService.App.Features.TokenLogin;

internal sealed record TokenLoginCommand(string RefreshToken) : IRequest<Result<JwtTokenPair>>;

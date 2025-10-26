namespace AuthenticationService.App.Features.Dto;

internal sealed record JwtTokenPair(string AccessToken, string RefreshToken);
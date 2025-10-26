namespace AuthenticationService.App.Common.Security.Tokens;

internal sealed record TokenPair(string AccessToken, RefreshTokenParameters RefreshToken);
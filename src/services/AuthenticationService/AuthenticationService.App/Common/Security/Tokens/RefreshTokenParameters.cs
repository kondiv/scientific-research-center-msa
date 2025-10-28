namespace AuthenticationService.App.Common.Security.Tokens;

internal sealed record RefreshTokenParameters(string Token, DateTime ExpiresAt);
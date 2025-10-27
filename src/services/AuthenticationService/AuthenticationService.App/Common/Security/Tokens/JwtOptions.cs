namespace AuthenticationService.App.Common.Security.Tokens;

public class JwtOptions
{
    public string Issuer { get; init; } = null!;
    
    public string Audience { get; init; } = null!;
    
    public string Key { get; init; } = null!;
    
    public int RefreshTokenLifetime { get; init; }
    
    public int AccessTokenLifetime { get; init; }
}
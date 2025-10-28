using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthenticationService.App.Common.Security.Tokens;
using AuthenticationService.App.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.App.Infrastructure.Security;

internal sealed class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _options;
    private readonly ILogger<JwtTokenProvider> _logger;

    public JwtTokenProvider(IOptions<JwtOptions> options, ILogger<JwtTokenProvider> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public TokenPair GetTokens(User user)
    {
        _logger.LogInformation("Generating tokens for user: {id}", user.Id);
        
        var accessToken = GenerateAccessToken(user);
        var refreshTokenParameters = GenerateRefreshToken();
        
        return new TokenPair(accessToken, refreshTokenParameters);
    }

    private string GenerateAccessToken(User user)
    {
        _logger.LogInformation("Generating access token for user: {id}", user.Id);
        var claims = new List<Claim>
        {
            new ("id", user.Id.ToString()),
            new (ClaimTypes.Email, user.Email),
            new (ClaimTypes.Role, user.Role.NormalizedName)
        };
        
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(_options.AccessTokenLifetime),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private RefreshTokenParameters GenerateRefreshToken()
    {
        _logger.LogInformation("Generating refresh token");
        return new RefreshTokenParameters(
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
            DateTime.UtcNow.AddSeconds(_options.RefreshTokenLifetime));
    }
}
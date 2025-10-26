using AuthenticationService.App.Common.Security.Tokens;
using AuthenticationService.App.Domain.Entities;
using AuthenticationService.App.Features.Dto;
using AuthenticationService.App.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.ResultPattern;
using Shared.ResultPattern.Errors;

namespace AuthenticationService.App.Features.TokenLogin;

internal sealed class TokenLoginCommandHandler : IRequestHandler<TokenLoginCommand, Result<JwtTokenPair>>
{
    private readonly AuthServiceContext _context;
    
    private readonly ITokenProvider _tokenProvider;
    
    private readonly ILogger<TokenLoginCommandHandler> _logger;

    public TokenLoginCommandHandler(AuthServiceContext context, ITokenProvider tokenProvider, ILogger<TokenLoginCommandHandler> logger)
    {
        _context = context;
        _tokenProvider = tokenProvider;
        _logger = logger;
    }

    public async Task<Result<JwtTokenPair>> Handle(TokenLoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login by token {token}", request.RefreshToken);
        var refreshToken = await _context
            .RefreshTokens
            .Include(rt => rt.User)
            .ThenInclude(u => u.Role)
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

        if (refreshToken is null or { Expired: true } or { Revoked: true })
        {
            _logger.LogError("Authentication error. Token was not found");
            return Result<JwtTokenPair>.Failure(new AuthError("Invalid refresh token"));
        }

        refreshToken.Revoke();

        var tokens = _tokenProvider.GetTokens(refreshToken.User);

        var newRefreshToken = new RefreshToken(
            tokens.RefreshToken.Token,
            tokens.RefreshToken.ExpiresAt,
            refreshToken.UserId);

        await _context.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<JwtTokenPair>.Success(new JwtTokenPair(tokens.AccessToken, newRefreshToken.Token));
    }
}
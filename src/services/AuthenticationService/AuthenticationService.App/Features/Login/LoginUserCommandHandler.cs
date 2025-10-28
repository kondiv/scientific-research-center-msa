using AuthenticationService.App.Common.Security;
using AuthenticationService.App.Common.Security.Tokens;
using AuthenticationService.App.Domain.Entities;
using AuthenticationService.App.Features.Dto;
using AuthenticationService.App.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.ResultPattern;
using Shared.ResultPattern.Errors;

namespace AuthenticationService.App.Features.Login;

internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<JwtTokenPair>>
{
    private readonly AuthServiceContext _context;
    
    private readonly IPasswordHasher _passwordHasher;
    
    private readonly ITokenProvider _tokenProvider;
    
    private readonly ILogger<LoginUserCommandHandler> _logger;

    public LoginUserCommandHandler(
        AuthServiceContext context,
        ILogger<LoginUserCommandHandler> logger,
        IPasswordHasher passwordHasher,
        ITokenProvider tokenProvider)
    {
        _context = context;
        _logger = logger;
        _passwordHasher = passwordHasher;
        _tokenProvider = tokenProvider;
    }

    public async Task<Result<JwtTokenPair>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login user.Login: {Login}", request.Login);
        
        var user = await _context
            .Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Login == request.Login, cancellationToken);

        if (user is null)
        {
            _logger.LogError("Failed to login: {Login}. Invalid login or password", request.Login);
            return Result<JwtTokenPair>.Failure(new AuthError("Invalid login or password. Please try again."));
        }

        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.HashPassword);

        if (!isPasswordValid)
        {
            _logger.LogError("Failed to login: {Login}. Invalid login or password", request.Login);
            return Result<JwtTokenPair>.Failure(new AuthError("Invalid login or password."));
        }

        var tokens = _tokenProvider.GetTokens(user);

        var refreshToken = new RefreshToken(tokens.RefreshToken.Token, tokens.RefreshToken.ExpiresAt, user.Id);

        await _context.AddAsync(refreshToken, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<JwtTokenPair>.Success(new JwtTokenPair(tokens.AccessToken, refreshToken.Token));
    }
}
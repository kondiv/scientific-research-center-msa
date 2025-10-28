using AuthenticationService.App.Common.Security;
using AuthenticationService.App.Domain.Entities;
using AuthenticationService.App.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Shared.ResultPattern;
using Shared.ResultPattern.Errors;

namespace AuthenticationService.App.Features.Register;

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    private readonly AuthServiceContext _context;
    
    private readonly IPasswordHasher _passwordHasher;
    
    private readonly ILogger<RegisterUserCommandHandler> _logger;

    public RegisterUserCommandHandler(
        AuthServiceContext context,
        ILogger<RegisterUserCommandHandler> logger,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[{dateTime:G}] Registering new user.\nEmail: {email}\nLogin: {login}\nRole: {role}",
            DateTime.UtcNow, request.Email, request.Login, request.NormalizedRoleName);

        var existingUser = await _context
            .Users
            .Where(u => u.Email == request.Email || u.Login == request.Login)
            .Select(u => new { u.Email, u.Login })
            .FirstOrDefaultAsync(cancellationToken);

        if (existingUser is not null)
        {
            if (existingUser.Email == request.Email)
            {
                _logger.LogError("User with email {email} already exists in database", request.Email);
                return Result<Guid>.Failure(new AlreadyExistsError("Email already in use"));
            }

            if (existingUser.Login == request.Login)
            {
                _logger.LogError("User with login {login} already exists in database", request.Login);
                return Result<Guid>.Failure(new AlreadyExistsError("Login already in use"));
            }
        }

        var role = await _context
            .Roles
            .FirstOrDefaultAsync(r => r.NormalizedName == request.NormalizedRoleName, cancellationToken);

        if (role is null)
        {
            _logger.LogError("Role with name {name} cannot be found", request.NormalizedRoleName);
            return Result<Guid>.Failure(new NotFoundError($"Cannot find role {request.NormalizedRoleName}"));
        }

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var user = new User(request.Email, request.Login, passwordHash, request.FullName, role.Id);
        
        await _context
            .Users
            .AddAsync(user, cancellationToken);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e)
            when(e.InnerException is NpgsqlException { SqlState: PostgresErrorCodes.UniqueViolation })
        {
            _logger.LogError(e, "Possible race condition. Email or login were taken before user finished" +
                                " registration");
            return Result<Guid>.Failure(new AlreadyExistsError("Registration failed. Please try again."));
        }

        return Result<Guid>.Success(user.Id);
    }
}
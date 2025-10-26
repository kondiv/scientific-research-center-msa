namespace AuthenticationService.App.Domain.Entities;

internal sealed class RefreshToken
{
    public Guid Id { get; private init; }
    public string Token { get; private init; }
    public DateTime ExpiresAt { get; private init; }
    public DateTime? RevokedAt { get; private set; }
    public bool Expired => ExpiresAt <= DateTime.UtcNow;
    public bool Revoked => RevokedAt is not null;
    public User User { get; private init; } = null!;
    public Guid UserId { get; private init; }

    public RefreshToken(string token, DateTime expiresAt, Guid userId)
    {
        Id = Guid.NewGuid();
        Token = token;
        ExpiresAt = expiresAt;
        UserId = userId;
    }

    public void Revoke()
    {
        if (Revoked)
        {
            throw new InvalidOperationException("Refresh token has already been revoked.");
        }
        
        RevokedAt = DateTime.UtcNow;
    }
}
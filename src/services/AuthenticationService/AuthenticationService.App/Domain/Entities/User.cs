namespace AuthenticationService.App.Domain.Entities;

internal sealed class User
{
    public Guid Id { get; private init; }

    public string Email { get; private set; }

    public string Login { get; private set; }
    
    public string HashPassword { get; private set; }

    public string FullName { get; private set; }

    public DateTime RegisteredAt { get; set; }

    public int RoleId { get; private set; }

    public Role Role { get; private set; } = null!;
    
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = [];

    public User(string email, string login, string hashPassword, string fullName, int roleId)
    {
        Id = Guid.NewGuid();
        Email = email;
        Login = login;
        HashPassword = hashPassword;
        FullName = fullName;
        RegisteredAt = DateTime.UtcNow;
        RoleId = roleId;
    }
}
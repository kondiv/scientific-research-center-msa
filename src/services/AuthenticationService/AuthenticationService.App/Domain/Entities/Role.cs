namespace AuthenticationService.App.Domain.Entities;

internal sealed class Role
{
    public Guid Id { get; private init; }
    
    public string Name { get; private set; }
    
    public string NormalizedName { get; private set; }

    public ICollection<User> Users { get; private set; } = [];

    public Role(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        NormalizedName = name.ToUpper();
    }
}
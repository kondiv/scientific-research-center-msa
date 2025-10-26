namespace AuthenticationService.App.Domain.Entities;

internal sealed class Role
{
    public int Id { get; init; }

    public string Name { get; set; } = null!;

    public string NormalizedName { get; set; } = null!;

    public ICollection<User> Users { get; private set; } = [];

    public Role()
    {
        
    }

    public Role(string name)
    {
        Name = name;
        NormalizedName = name.ToUpper();
    }
}
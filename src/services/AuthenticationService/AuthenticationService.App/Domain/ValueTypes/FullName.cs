namespace AuthenticationService.App.Domain.ValueTypes;

internal sealed record FullName(string Name, string Surname, string? Patronymic)
{
    public override string ToString()
    {
        return string.Join(' ', 
            new[] { Name, Surname, Patronymic } 
            .Where(s => !string.IsNullOrEmpty(s))).Trim();
    }
}
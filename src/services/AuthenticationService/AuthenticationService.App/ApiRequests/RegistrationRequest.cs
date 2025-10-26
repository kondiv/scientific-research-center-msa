using AuthenticationService.App.Domain.ValueTypes;

namespace AuthenticationService.App.ApiRequests;

public sealed record RegistrationRequest(string Email, string Login, FullName FullName, string Password,
    string NormalizedRoleName);
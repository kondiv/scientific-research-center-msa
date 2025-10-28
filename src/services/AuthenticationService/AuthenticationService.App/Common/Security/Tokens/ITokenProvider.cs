using AuthenticationService.App.Domain.Entities;

namespace AuthenticationService.App.Common.Security.Tokens;

internal interface ITokenProvider
{
    TokenPair GetTokens(User user);
}
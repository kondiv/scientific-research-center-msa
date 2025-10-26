using AuthenticationService.App.ApiRequests;
using AuthenticationService.App.Features.Login;
using AuthenticationService.App.Features.Register;
using AuthenticationService.App.Features.TokenLogin;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.App.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> RegisterAsync(RegistrationRequest request, CancellationToken cancellationToken = default)
    {
        var registrationCommand = new RegisterUserCommand(request.FullName.ToString(), request.Email, request.Login,
            request.Password, request.NormalizedRoleName);

        var result = await _mediator.Send(registrationCommand, cancellationToken);

        if (result.Succeeded)
        {
            return Ok(result.Value);
        }

        return result.Error.ErrorCode switch
        {
            _ => Conflict(result.Error.Message)
        };
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var loginCommand = new LoginUserCommand(request.Login, request.Password);
        
        var result = await _mediator.Send(loginCommand, cancellationToken);

        if (result.IsFailure)
        {
            return Unauthorized(result.Error.Message);
        }
        
        AppendTokensToCookies(HttpContext, result.Value.AccessToken, result.Value.RefreshToken);
        return NoContent();
    }

    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> RefreshAsync(CancellationToken cancellationToken = default)
    {
        HttpContext.Request.Cookies.TryGetValue("refresh_token", out var refreshToken);

        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized();
        }           
        
        var command = new TokenLoginCommand(refreshToken);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Unauthorized(result.Error.Message);
        }
        
        AppendTokensToCookies(HttpContext, result.Value.AccessToken, result.Value.RefreshToken);
        
        return NoContent();
    }

    private static void AppendTokensToCookies(HttpContext context, string accessToken, string refreshToken)
    {
        context.Response.Cookies.Append("access_token", accessToken, new CookieOptions()
        {
            Secure = true,
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddSeconds(180),
            SameSite = SameSiteMode.None
        });
        
        context.Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions()
        {
            Secure = true,
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddSeconds(2592000),
            SameSite = SameSiteMode.None
        });
    }
}
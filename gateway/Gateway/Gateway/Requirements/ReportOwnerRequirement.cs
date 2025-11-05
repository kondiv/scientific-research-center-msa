using Microsoft.AspNetCore.Authorization;

namespace Gateway.Requirements;

internal sealed record ReportOwnerRequirement() : IAuthorizationRequirement;

internal sealed class ReportOwnerRequirementHandler : AuthorizationHandler<ReportOwnerRequirement>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ReportOwnerRequirementHandler> _logger;

    public ReportOwnerRequirementHandler(
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor,
        ILogger<ReportOwnerRequirementHandler> logger)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ReportOwnerRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
       
        if (httpContext is null)
        {
            _logger.LogError("HttpContext is null");
            return;
        }

        if (!httpContext.Request.RouteValues.TryGetValue("id", out var reportId))
        {
            _logger.LogError("Route value 'id' is null");
            return;
        }

        var userIdClaim = httpContext.User.FindFirst("id");
        if (userIdClaim is null)
        {
            _logger.LogError("User id claim is null");
            return;
        }

        var httpClient = _httpClientFactory.CreateClient();

        try
        {
            var response = await httpClient.GetAsync($"http://scientific-reports-service:5002/reports/{reportId}/author");

            if (response.IsSuccessStatusCode)
            {
                var authorData = await response.Content.ReadFromJsonAsync<AuthorResponse>();

                if (authorData is null)
                {
                    _logger.LogError("Author data is null");
                    return;
                }
                
                if (string.IsNullOrEmpty(authorData.Id))
                {
                    _logger.LogError("Author data 'id' is null");
                }

                if (string.IsNullOrEmpty(authorData.Name))
                {
                    _logger.LogError("Author data 'name' is null");
                }
                
                _logger.LogInformation("Got author. Id: {id}, Name: {name}", authorData.Id, authorData.Name);
                if (authorData.Id == userIdClaim.Value)
                {
                    context.Succeed(requirement);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception was thrown");
        }
    }
}

internal sealed class AuthorResponse
{
    public string Id { get; init; } = null!;
    public string Name { get; init; } = null!;
}


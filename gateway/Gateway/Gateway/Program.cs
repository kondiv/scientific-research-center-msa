using Gateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddHttpContextAccessor();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddGateway(builder.Configuration);

builder.Services.AddPolicies();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapReverseProxy();

app.Run();
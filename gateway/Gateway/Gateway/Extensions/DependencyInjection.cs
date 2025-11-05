using System.Text;
using Gateway.Requirements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Gateway.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddGateway(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddReverseProxy().LoadFromConfig(configuration.GetSection("ReverseProxy"));
        
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"]!,
                    ValidAudience = configuration["Jwt:Audience"]!,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };

                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.TryGetValue("access_token", out var token))
                        {
                            context.Token = token;
                        }
                
                        return Task.CompletedTask;
                    }
                };
            });
        
        return services;
    }

    public static IServiceCollection AddPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("Admin", policy => policy.RequireRole("ADMIN"));
        
        services.AddAuthorizationBuilder()
            .AddPolicy("Scientist", policy => policy.RequireRole("SCIENTIST", "ADMIN"));

        services.AddAuthorizationBuilder()
            .AddPolicy("ReportOwner", policy => 
                policy
                    .RequireRole("ADMIN")
                    .AddRequirements(new ReportOwnerRequirement()));

        services.AddTransient<IAuthorizationHandler, ReportOwnerRequirementHandler>();

        return services;
    }
}
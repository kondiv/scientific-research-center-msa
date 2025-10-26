using System.Text;
using AuthenticationService.App.Common.Security;
using AuthenticationService.App.Common.Security.Tokens;
using AuthenticationService.App.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.App.Extensions.DependencyInjection;

internal static class DependencyInjection
{
    public static IServiceCollection AddRequiredServices(this IServiceCollection services)
    {
        services.AddMediatR(options => 
            options.RegisterServicesFromAssembly(typeof(Program).Assembly));

        services.AddTransient<IPasswordHasher, BCryptPasswordHasher>();
        services.AddTransient<ITokenProvider, JwtTokenProvider>();
        
        return services;
    }

    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
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
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };

                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        context.Request.Cookies.TryGetValue("access_token", out var token);
                        if (token is not null)
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        
        return services;
    }
}
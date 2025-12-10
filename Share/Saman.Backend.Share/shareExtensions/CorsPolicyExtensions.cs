using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class CorsPolicyExtensions
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration, string myAllowSpecificOrigins)
    {
        IConfigurationSection allowedOriginsSection = configuration.GetSection("AllowedOrigins");
        string[] allowedOrigins = allowedOriginsSection.Get<string[]>()!;

        services.AddCors(options =>
        {
            options.AddPolicy(name: myAllowSpecificOrigins,
                policy =>
                {
                    policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowed(orgin => _ = true);
                });
        });

        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddScopeServices(this IServiceCollection services)
    {
        services.AddScoped<Saman.Backend.Share.shareServices.CurrentUser_Service>();
        services.AddScoped<Saman.Backend.Business.baseServices.CRUD_Service>();
        services.AddTransient<Saman.Backend.Business.Entity.Category.Category_Service>();
        services.AddTransient<Saman.Backend.Business.Entity.Product.Product_Service>();
        services.AddTransient<Saman.Backend.Business.Entity.Log.Log_Service>();

        return services;
    }
}

using Microsoft.Extensions.DependencyInjection;
using OsService.Services.Base;

namespace OsService.Services;

/// <summary>
/// Registrar of services in services provider.
/// </summary>
public static class DiInitializer
{
    /// <summary>
    /// Register services in services provider.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IInfoService<string>, OsInfoService>();
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace OsService.Queries;

/// <summary>
/// Registrar of services in services provider.
/// </summary>
public static class DiInitializer
{
    /// <summary>
    /// Register services in services provider.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    public static void AddQueries(this IServiceCollection services)
    {
        services.AddScoped<ConfigurationQuery>();
        services.AddScoped<ConnectedPciHardwareQuery>();
        services.AddScoped<UsbHardwareQuery>();
        services.AddScoped<HostNameQuery>();
        services.AddScoped<InstalledSoftwareQuery>();
        services.AddScoped<SoftwareInfoQuery>();
        services.AddScoped<UpgradableSoftwareQuery>();
    }
}
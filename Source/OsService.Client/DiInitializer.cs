using Microsoft.Extensions.DependencyInjection;

namespace OsService.Client;

/// <summary>
/// Registrar of services in services provider.
/// </summary>
public static class DiInitializer
{
    /// <summary>
    /// Register services in services provider.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    public static void AddClient(this IServiceCollection services)
    {
        // services.AddScoped<>();
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace OsService.Commands;

/// <summary>
/// Registrar of services in services provider.
/// </summary>
public static class DiInitializer
{
    /// <summary>
    /// Register services in services provider.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    public static void AddCommands(this IServiceCollection services)
    {
        // services.AddScoped<>();
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace Server.Queries;

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
        services.AddScoped<HostIpsWithEaServiceQuery>();
    }
}
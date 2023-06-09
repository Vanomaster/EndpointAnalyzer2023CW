using Microsoft.Extensions.DependencyInjection;

namespace Analyzers;

/// <summary>
/// Registrar of services in services provider.
/// </summary>
public static class DiInitializer
{
    /// <summary>
    /// Register services in services provider.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    public static void AddAnalyzers(this IServiceCollection services)
    {
        services.AddScoped<ConfigurationAnalyzer>();
        services.AddScoped<HardwareTrustAnalyzer>();
        services.AddScoped<SoftwareTrustAnalyzer>();
        services.AddScoped<SoftwareUpdateAnalyzer>();
    }
}
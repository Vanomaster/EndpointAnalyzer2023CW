using CleanModels.Hardware;
using CleanModels.Network;
using CleanModels.Software;
using Microsoft.Extensions.DependencyInjection;
using Parsers.Base;

namespace Parsers;

/// <summary>
/// Registrar of services in services provider.
/// </summary>
public static class DiInitializer
{
    /// <summary>
    /// Register services in services provider.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    public static void AddParsers(this IServiceCollection services)
    {
        services.AddScoped<IParser<string, List<Host>>, HostsParser>();
        services.AddScoped<IParser<string, List<UnknownHardware>>, InstalledHardwareParser>();
        services.AddScoped<IParser<string, List<Software>>, InstalledSoftwareParser>();
        services.AddScoped<IParser<string, List<UpgradableSoftware>>, UpgradableSoftwareParser>();
    }
}
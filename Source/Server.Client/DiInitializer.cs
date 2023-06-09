using Microsoft.Extensions.DependencyInjection;
using OsInfoService;
using Server.Client.Clients;
using Server.Client.Clients.Base;
using TestService;

namespace Server.Client;

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
        services.AddSingleton<IChannelProvider, ChannelProvider>();
        //services.AddScoped<ServiceClientBase<Test.TestClient>, TestServiceClient>();
        services.AddScoped<IConfigurationInfoServiceClient, ConfigurationInfoServiceClient>();
        services.AddScoped<HostNameInfoServiceClient>();
        services.AddScoped<IUsbHardwareInfoServiceClient, UsbHardwareInfoServiceClient>();
        services.AddScoped<IInstalledSoftwareInfoServiceClient, InstalledSoftwareInfoServiceClient>();
        services.AddScoped<IUpgradableSoftwareInfoServiceClient, UpgradableSoftwareInfoServiceClient>();
    }
}
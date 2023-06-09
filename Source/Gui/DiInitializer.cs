using Gui.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Gui;

/// <summary>
/// Registrar of services in services provider.
/// </summary>
public static class DiInitializer
{
    /// <summary>
    /// Register services in services provider.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    public static void AddGui(this IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();

        // services.AddScoped<BenchmarksModel>();
        // services.AddScoped<>();
        // services.AddScoped<>();
    }
}
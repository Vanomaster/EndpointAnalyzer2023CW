using Cli.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Cli;

/// <summary>
/// Registrar of services in services provider.
/// </summary>
public static class DiInitializer
{
    /// <summary>
    /// Register services in services provider.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    public static void AddCli(this IServiceCollection services)
    {
        services.AddSingleton<IHandler, InputHandler>();
    }
}
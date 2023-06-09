using Client.Services.Base;
using Client.Services.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Services;

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
        services.AddScoped(typeof(IParser<,>), typeof(FileParser<,>));
        services.AddScoped<TestConnectionService>();
    }
}
using Dal.Entities;
using Dal.EqualityComparers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dal;

/// <summary>
/// Registrar of services in services provider.
/// </summary>
public static class DiInitializer
{
    /// <summary>
    /// Register services in services provider.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    /// <returns>Changed collection of services.</returns>
    public static IServiceCollection AddDal(this IServiceCollection services)
    {
        services.AddDbContextFactory<Context>(GetOptions()); // , ServiceLifetime.Transient
        services.AddScoped<IConfigurationHelper, ConfigurationHelper>();
        services.AddScoped<IEqualityComparer<AnalysisScheduleRecord>, AnalysisScheduleRecordComparer>();
        services.AddScoped<IEqualityComparer<Benchmark>, BenchmarkComparer>();
        services.AddScoped<IEqualityComparer<Configuration>, ConfigurationComparer>();
        services.AddScoped<IEqualityComparer<ConfigurationRecommendation>, ConfigurationRecommendationComparer>();
        services.AddScoped<IEqualityComparer<ConfigurationRecommendationsBenchmark>, ConfigurationRecommendationsBenchmarkComparer>();
        services.AddScoped<IEqualityComparer<TrustedHardwareBenchmark>, TrustedHardwareBenchmarkComparer>();
        services.AddScoped<IEqualityComparer<TrustedHardware>, TrustedHardwareComparer>();
        services.AddScoped<IEqualityComparer<TrustedSoftwareBenchmark>, TrustedSoftwareBenchmarkComparer>();
        services.AddScoped<IEqualityComparer<TrustedSoftware>, TrustedSoftwareComparer>();

        return services;
    }

    private static Action<DbContextOptionsBuilder> GetOptions()
    {
        var configuration = new ConfigurationHelper();

        return options => options.UseNpgsql(configuration.MainConnectionString).EnableSensitiveDataLogging();
    }
}
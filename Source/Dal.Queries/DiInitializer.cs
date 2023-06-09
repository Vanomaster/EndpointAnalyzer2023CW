using CleanModels.Queries.Base;
using Dal.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Dal.Queries;

/// <summary>
/// Registrar of services in services provider.
/// </summary>
public static class DiInitializer
{
    /// <summary>
    /// Register services in services provider.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    public static void AddDalQueries(this IServiceCollection services)
    {
        // services.AddScoped(typeof(EntitiesQuery<,>));
        services.AddScoped<AnalysisResultQuery>();
        services.AddScoped<AnalysisScheduleRecordByPcIpAnyQuery>();
        services.AddScoped<AnalysisScheduleRecordPcIpQuery>();
        services.AddScoped<AnalysisScheduleRecordQuery>();
        services.AddScoped(typeof(AllQuery<>));
        services.AddScoped(typeof(AllNamesQuery<>));
        // services.AddScoped(typeof(AllQuery<>));
        // services.AddScoped(typeof(AllQuery<>));
        services.AddScoped<BenchmarksPageQuery>();
        services.AddScoped<PcQuantityByBenchmarkNameQuery>();
        services.AddScoped<ConfigurationRecommendationsBenchmarkQuery>();
        services.AddScoped<ConfigurationRecommendationsPageQuery>();
        services.AddScoped<TrusHardBenchmarkQuery>();
        services.AddScoped<TrusSoftBenchmarkQuery>();
        services.AddScoped<TrustHardPageQuery>();
        services.AddScoped<TrustSoftPageQuery>();

        services.AddScoped<IQuery<string, List<ConfigurationRecommendation>>, AllConfigurationRecommendationsByBenchmarkNameQuery>();
        services.AddScoped<IQuery<string, List<TrustedHardware>>, TrustedHardwareByBenchmarkNameQuery>();
        services.AddScoped<IQuery<string, List<TrustedSoftware>>, TrustedSoftwareByBenchmarkNameQuery>();
    }
}
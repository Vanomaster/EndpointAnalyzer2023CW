using Dal.Entities;
using Microsoft.Extensions.DependencyInjection;
using Server.Services.Base;
using AnalysisScheduleRecordModel = CleanModels.Schedule.AnalysisScheduleRecord;
using BenchmarkModel = CleanModels.Benchmark.Benchmark;

namespace Server.Services;

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
        services.AddScoped<IAnalysisResultService, AnalysisResultService>();
        services.AddScoped<IAnalysisService, AnalysisService>();
        services.AddScoped<IDbService<BenchmarkModel>, BenchmarkService>();
        services.AddScoped<IDbService<ConfigurationRecommendationsBenchmark>, ConfigurationRecommendationsBenchmarkService>();
        services.AddScoped<IDbMultipleService<ConfigurationRecommendation>, ConfigurationRecommendationsService>();
        services.AddScoped<ConfigurationRecommendationsService>();
        services.AddScoped<TrustHardBenchmarkService>();
        services.AddScoped<TrustHardService>();
        services.AddScoped<TrustSoftBenchmarkService>();
        services.AddScoped<TrustSoftService>();
        //services.AddScoped<IDbService<TrustedSoftwareBenchmark>, TrustedSoftwareBenchmarkService>();
        //services.AddScoped<IDbService<TrustedHardwareBenchmark>, TrustedHardwareBenchmarkService>();
        services.AddScoped<IScheduleService<AnalysisScheduleRecordModel>, ScheduleService>();
        services.AddScoped<InitService>();
        services.AddScoped<INetworkService, NetworkService>();
        services.AddSingleton<HostsWithEaService>();
    }
}
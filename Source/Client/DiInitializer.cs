using CleanModels;
using CleanModels.Benchmark;
using Client.Clients;
using Client.Clients.Base;
using ConfigurationRecommendationsService;
using Microsoft.Extensions.DependencyInjection;
using static AnalysisResultService.AnalysisResult;
using static AnalysisService.Analysis;
using static BenchmarkService.Benchmark;
using static NetworkService.Network;
using static ScheduleService.Schedule;
using static TestService.Test;
using static TrustedHardwareService.TrustedHardware;
using static TrustedSoftwareService.TrustedSoftware;
using AnalysisResultModel = CleanModels.Analysis.AnalysisResult;
using ConfigurationRecommendationsBenchmark = ConfigurationRecommendationsBenchmarkService.ConfigurationRecommendationsBenchmark;
using HostModel = CleanModels.Network.Host;
using TrustedHardwareBenchmark = TrustedHardwareBenchmarkService.TrustedHardwareBenchmark;
using TrustedSoftwareBenchmark = TrustedSoftwareBenchmarkService.TrustedSoftwareBenchmark;

namespace Client;

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
        var configuration = new ConfigurationHelper();
        services.AddScoped<IConfigurationHelper, ConfigurationHelper>();
        services.AddSingleton<IChannelProvider>(_ => new ChannelProvider(configuration.ServerAddress));
        services.AddSingleton<TestServiceClient>();
        services.AddSingleton<TestClient>(serviceProvider =>
            new TestClient(serviceProvider.GetRequiredService<IChannelProvider>().AcquireChannel()));

        services.AddSingleton<
            IReadOnlyServiceClient<AnalysisResultClient, PageModel, AnalysisResultModel>,
            AnalysisResultServiceClient>();

        services.AddSingleton<AnalysisResultClient>(serviceProvider =>
            new AnalysisResultClient(serviceProvider.GetRequiredService<IChannelProvider>().AcquireChannel()));

        services.AddSingleton<AnalysisServiceClient>();
        services.AddSingleton<AnalysisClient>(serviceProvider =>
            new AnalysisClient(serviceProvider.GetRequiredService<IChannelProvider>().AcquireChannel()));

        services.AddSingleton<IEntityServiceClient<BenchmarkClient, Benchmark>, BenchmarkServiceClient>();
        services.AddSingleton<BenchmarkClient>(serviceProvider =>
            new BenchmarkClient(serviceProvider.GetRequiredService<IChannelProvider>().AcquireChannel()));

        services.AddSingleton<IReadOnlyServiceClient<NetworkClient, HostModel>, NetworkServiceClient>();
        services.AddSingleton<NetworkClient>(serviceProvider =>
            new NetworkClient(serviceProvider.GetRequiredService<IChannelProvider>().AcquireChannel()));

        services.AddSingleton<ScheduleServiceClient>();
        services.AddSingleton<ScheduleClient>(serviceProvider =>
            new ScheduleClient(serviceProvider.GetRequiredService<IChannelProvider>().AcquireChannel()));

        services.AddSingleton<ConfigurationRecommendationsServiceClient>();
        services.AddSingleton<ConfigurationRecommendations.ConfigurationRecommendationsClient>(serviceProvider =>
            new ConfigurationRecommendations.ConfigurationRecommendationsClient(serviceProvider.GetRequiredService<IChannelProvider>().AcquireChannel()));

        services.AddSingleton<ConfigurationRecommendationsBenchmarkServiceClient>();
        services.AddSingleton<ConfigurationRecommendationsBenchmark.ConfigurationRecommendationsBenchmarkClient>(serviceProvider =>
            new ConfigurationRecommendationsBenchmark.ConfigurationRecommendationsBenchmarkClient(serviceProvider.GetRequiredService<IChannelProvider>().AcquireChannel()));

        services.AddSingleton<TrustSoftServiceClient>();
        services.AddSingleton<TrustedSoftwareClient>(serviceProvider =>
            new TrustedSoftwareClient(serviceProvider.GetRequiredService<IChannelProvider>().AcquireChannel()));

        services.AddSingleton<TrustHardServiceClient>();
        services.AddSingleton<TrustedHardwareClient>(serviceProvider =>
            new TrustedHardwareClient(serviceProvider.GetRequiredService<IChannelProvider>().AcquireChannel()));

        services.AddSingleton<TrustSoftBenchmarkServiceClient>();
        services.AddSingleton<TrustedSoftwareBenchmark.TrustedSoftwareBenchmarkClient>(serviceProvider =>
            new TrustedSoftwareBenchmark.TrustedSoftwareBenchmarkClient(serviceProvider.GetRequiredService<IChannelProvider>().AcquireChannel()));

        services.AddSingleton<TrustHardBenchmarkServiceClient>();
        services.AddSingleton<TrustedHardwareBenchmark.TrustedHardwareBenchmarkClient>(serviceProvider =>
            new TrustedHardwareBenchmark.TrustedHardwareBenchmarkClient(serviceProvider.GetRequiredService<IChannelProvider>().AcquireChannel()));
    }

    // /// <summary>
    // /// Register services in services provider.
    // /// </summary>
    // /// <param name="services">Collection of services.</param>
    // public static void AddGrpcClients(this IServiceCollection services)
    // {
    //
    // }
}
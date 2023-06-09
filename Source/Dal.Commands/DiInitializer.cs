using Dal.Commands.Test;
using Microsoft.Extensions.DependencyInjection;

namespace Dal.Commands;

/// <summary>
/// Registrar of services in services provider.
/// </summary>
public static class DiInitializer
{
    /// <summary>
    /// Register services in services provider.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    public static void AddDalCommands(this IServiceCollection services)
    {
        services.AddScoped<TestConnectionCommand>();
        services.AddScoped(typeof(AddOrUpdateEntityCommand<>));
        // services.AddScoped(typeof(AddOrUpdateUniqueEntityCommand<,>));
        services.AddScoped(typeof(AddOnlyNameUniqueEntityCommand<>));
        services.AddScoped(typeof(UpdateOnlyNameUniqueEntityCommand<>));
        //services.AddScoped(typeof(AddNewNamedEntitiesCommand<>));
        //services.AddScoped(typeof(UpdateNamedEntitiesCommand<>));
        services.AddScoped(typeof(RemoveEntitiesCommand<>));
        services.AddScoped<AddConfigurationRecommendationCommand>();
        services.AddScoped<AddConfigurationRecommendationsBenchmarkCommand>();
        services.AddScoped<AddNewTrustHardCommand>();
        services.AddScoped<AddNewTrustSoftCommand>();
        services.AddScoped<RemoveConfigCommand>();
        services.AddScoped<RemoveSoftCommand>();
        services.AddScoped<RemoveHardCommand>();
        services.AddScoped<AddTrustHardBenchmarkCommand>();
        services.AddScoped<AddTrustSoftBenchmarkCommand>();
        services.AddScoped<UpdateConfigurationRecommendationCommand>();
        services.AddScoped<UpdateConfigurationRecommendationsBenchmarkCommand>();
    }
}
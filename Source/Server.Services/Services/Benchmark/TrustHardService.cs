using CleanModels;
using CleanModels.Commands.Base;
using CleanModels.Queries.Base;
using Dal.Commands;
using Dal.Entities;
using Dal.Queries;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Server.Services.Base;
using ConfigurationRecommendation = Dal.Entities.ConfigurationRecommendation;

namespace Server.Services;

public class TrustHardService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationRecommendationsService"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    public TrustHardService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }

    public async Task<QueryResult<List<TrustedHardware>>> GetPageAsync(PageModel pageModel)
    {
        Logger.Info("ConfigurationRecommendationsBenchmarkService Get started.");
        using var scope = ServiceProvider.CreateScope();
        var benchmarkQuery = scope.ServiceProvider.GetRequiredService<TrustHardPageQuery>();
        var queryResult = await benchmarkQuery.ExecuteAsync(pageModel);
        if (!queryResult.IsSuccessful)
        {
            Logger.Error($"ConfigurationRecommendationsBenchmarkService Get failed. {queryResult.ErrorMessage}");

            return new QueryResult<List<TrustedHardware>>(queryResult.ErrorMessage);
        }

        Logger.Info("ConfigurationRecommendationsBenchmarkService Get successfully completed.");

        return new QueryResult<List<TrustedHardware>>(queryResult.Data);
    }

    // public async Task<QueryResult<List<ConfigurationRecommendation>>> GetExistAsync(
    //     List<ConfigurationRecommendation> model)
    // {
    //     Logger.Info("BenchmarkService Get started.");
    //     using var scope = ServiceProvider.CreateScope();
    //     var query = scope.ServiceProvider.GetRequiredService<ExistConfigurationRecommendationsQuery>();
    //     var queryResult = await query.ExecuteAsync(model);
    //     if (!queryResult.IsSuccessful)
    //     {
    //         Logger.Error($"BenchmarkService Get failed. {queryResult.ErrorMessage}");
    //
    //         return new QueryResult<List<ConfigurationRecommendation>>(queryResult.ErrorMessage);
    //     }
    //
    //     Logger.Info("BenchmarkService Get successfully completed.");
    //
    //     return new QueryResult<List<ConfigurationRecommendation>>(queryResult.Data);
    // }

    public async Task<CommandResult> AddAsync(List<TrustedHardware> model)
    {
        Logger.Info("ConfigurationRecommendationsBenchmarkService Add started.");
        using var scope = ServiceProvider.CreateScope();
        var addCommand = scope.ServiceProvider
            .GetRequiredService<AddNewTrustHardCommand>();

        var commandResult = await addCommand.ExecuteAsync(model);
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"ConfigurationRecommendationsBenchmarkService Add failed. {commandResult.ErrorMessage}");

            return new CommandResult(commandResult.ErrorMessage);
        }

        Logger.Info("ConfigurationRecommendationsBenchmarkService Add successfully completed.");

        return new CommandResult();
    }

    // public async Task<CommandResult> UpdateAsync(List<ConfigurationRecommendation> model)
    // {
    //     Logger.Info("ConfigurationRecommendationsBenchmarkService Update started.");
    //     using var scope = ServiceProvider.CreateScope();
    //     var command = scope.ServiceProvider
    //         .GetRequiredService<UpdateConfigurationRecommendationCommand>();
    //
    //     var commandResult = await command.ExecuteAsync(model);
    //     if (!commandResult.IsSuccessful)
    //     {
    //         Logger.Error($"ConfigurationRecommendationsBenchmarkService Update failed. {commandResult.ErrorMessage}");
    //
    //         return new CommandResult(commandResult.ErrorMessage);
    //     }
    //
    //     Logger.Info("ConfigurationRecommendationsBenchmarkService Update successfully completed.");
    //
    //     return new CommandResult();
    // }

    /// <inheritdoc/>s
    public async Task<CommandResult> RemoveAsync(IEnumerable<TrustedHardware> model)
    {
        Logger.Info("ConfigurationRecommendationsBenchmarkService Update started.");
        using var scope = ServiceProvider.CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<RemoveEntitiesCommand<TrustedHardware>>();
        var commandResult = await command.ExecuteAsync(model);
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"ConfigurationRecommendationsBenchmarkService Update failed. {commandResult.ErrorMessage}");

            return new CommandResult(commandResult.ErrorMessage);
        }

        Logger.Info("ConfigurationRecommendationsBenchmarkService Update successfully completed.");

        return new CommandResult();
    }
}
using CleanModels;
using CleanModels.Commands.Base;
using CleanModels.Queries.Base;
using Dal.Commands;
using Dal.Entities;
using Dal.Queries;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Server.Services.Base;
using Benchmark = Dal.Entities.TrustedHardwareBenchmark;

namespace Server.Services;

public class TrustHardBenchmarkService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationRecommendationsBenchmarkService"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    public TrustHardBenchmarkService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }

    /// <inheritdoc/>
    public async Task<QueryResult<Benchmark>> GetOneAsync(string benchmarkName)
    {
        Logger.Info("ConfigurationRecommendationsBenchmarkService Get started.");
        using var scope = ServiceProvider.CreateScope();
        var benchmarkQuery = scope.ServiceProvider.GetRequiredService<TrusHardBenchmarkQuery>();
        var queryResult = await benchmarkQuery.ExecuteAsync(benchmarkName);
        if (!queryResult.IsSuccessful)
        {
            Logger.Error($"ConfigurationRecommendationsBenchmarkService Get failed. {queryResult.ErrorMessage}");

            return new QueryResult<Benchmark>(queryResult.ErrorMessage);
        }

        Logger.Info("ConfigurationRecommendationsBenchmarkService Get successfully completed.");

        return new QueryResult<Benchmark>(queryResult.Data);
    }
    //
    // public async Task<QueryResult<List<string>>> GetAllNamesAsync()
    // {
    //     Logger.Info("BenchmarkService Get started.");
    //     using var scope = ServiceProvider.CreateScope();
    //     var query = scope.ServiceProvider.GetRequiredService<AllNamesQuery<Benchmark>>();
    //     var queryResult = await query.ExecuteAsync();
    //     if (!queryResult.IsSuccessful)
    //     {
    //         Logger.Error($"BenchmarkService Get failed. {queryResult.ErrorMessage}");
    //
    //         return new QueryResult<List<string>>(queryResult.ErrorMessage);
    //     }
    //
    //     Logger.Info("BenchmarkService Get successfully completed.");
    //
    //     return new QueryResult<List<string>>(queryResult.Data);
    // }

    public async Task<CommandResult> AddAsync(Benchmark model)
    {
        Logger.Info("ConfigurationRecommendationsBenchmarkService Add started.");
        using var scope = ServiceProvider.CreateScope();
        var addCommand = scope.ServiceProvider.GetRequiredService<AddTrustHardBenchmarkCommand>();
        var commandResult = await addCommand.ExecuteAsync(model);
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"ConfigurationRecommendationsBenchmarkService Add failed. {commandResult.ErrorMessage}");

            return new CommandResult(commandResult.ErrorMessage);
        }

        Logger.Info("ConfigurationRecommendationsBenchmarkService Add successfully completed.");

        return new CommandResult();
    }
    //
    // public async Task<CommandResult> UpdateAsync(Benchmark model)
    // {
    //     Logger.Info("ConfigurationRecommendationsBenchmarkService Update started.");
    //     using var scope = ServiceProvider.CreateScope();
    //     var command = scope.ServiceProvider
    //         .GetRequiredService<UpdateOnlyNameUniqueEntityCommand<Benchmark>>();
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
    //
    // /// <inheritdoc/>s
    // public async Task<CommandResult> RemoveAsync(IEnumerable<ConfigurationRecommendationsBenchmark> model)
    // {
    //     Logger.Info("ConfigurationRecommendationsBenchmarkService Update started.");
    //     using var scope = ServiceProvider.CreateScope();
    //     var command = scope.ServiceProvider.GetRequiredService<RemoveEntitiesCommand<Benchmark>>();
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
}
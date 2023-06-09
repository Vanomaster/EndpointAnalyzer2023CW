using CleanModels;
using CleanModels.Commands.Base;
using CleanModels.Queries.Base;
using Dal.Commands;
using Dal.Queries;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Server.Services.Base;
using BenchmarkEntity = Dal.Entities.Benchmark;
using BenchmarkModel = CleanModels.Benchmark.Benchmark;

namespace Server.Services;

public class BenchmarkService : IDbService<BenchmarkModel>
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="BenchmarkService"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    public BenchmarkService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }

    public async Task<QueryResult<BenchmarkModel>> GetOneAsync(string name)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<QueryResult<List<BenchmarkModel>>> GetPageAsync(PageModel pageModel)
    {
        Logger.Info("BenchmarkService Get started.");
        using var scope = ServiceProvider.CreateScope();
        var benchmarkQuery = scope.ServiceProvider.GetRequiredService<BenchmarksPageQuery>();
        var queryResult = await benchmarkQuery.ExecuteAsync(pageModel);
        if (!queryResult.IsSuccessful)
        {
            Logger.Error($"BenchmarkService Get failed. {queryResult.ErrorMessage}");

            return new QueryResult<List<BenchmarkModel>>(queryResult.ErrorMessage);
        }

        var benchmarks = queryResult.Data.Select(b => b.MapToBenchmarkModel()).ToList();
        foreach (var benchmark in benchmarks)
        {
            var pcQuantityQuery = scope.ServiceProvider.GetRequiredService<PcQuantityByBenchmarkNameQuery>();
            var pcQuantityQueryResult = await pcQuantityQuery.ExecuteAsync(benchmark.Name);
            if (!pcQuantityQueryResult.IsSuccessful)
            {
                Logger.Error($"BenchmarkService Get failed. {pcQuantityQueryResult.ErrorMessage}");

                return new QueryResult<List<BenchmarkModel>>(pcQuantityQueryResult.ErrorMessage);
            }

            benchmark.ComputerQuantity = pcQuantityQueryResult.Data;
        }

        Logger.Info("BenchmarkService Get successfully completed.");

        return new QueryResult<List<BenchmarkModel>>(benchmarks);
    }

    public async Task<QueryResult<List<string>>> GetAllNamesAsync()
    {
        Logger.Info("BenchmarkService Get started.");
        using var scope = ServiceProvider.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<AllNamesQuery<BenchmarkEntity>>();
        var queryResult = await query.ExecuteAsync();
        if (!queryResult.IsSuccessful)
        {
            Logger.Error($"BenchmarkService Get failed. {queryResult.ErrorMessage}");

            return new QueryResult<List<string>>(queryResult.ErrorMessage);
        }

        Logger.Info("BenchmarkService Get successfully completed.");

        return new QueryResult<List<string>>(queryResult.Data);
    }

    /// <inheritdoc/>
    public async Task<CommandResult> AddAsync(BenchmarkModel model)
    {
        Logger.Info("BenchmarkService Add started.");
        using var scope = ServiceProvider.CreateScope();
        var addCommand = scope.ServiceProvider.GetRequiredService<AddOnlyNameUniqueEntityCommand<BenchmarkEntity>>();
        var commandResult = await addCommand.ExecuteAsync(model.MapToBenchmarkEntity());
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"BenchmarkService Add failed. {commandResult.ErrorMessage}");

            return new CommandResult(commandResult.ErrorMessage);
        }

        Logger.Info("BenchmarkService Add successfully completed.");

        return new CommandResult();
    }

    /// <inheritdoc/>
    public async Task<CommandResult> UpdateAsync(BenchmarkModel model)
    {
        Logger.Info("BenchmarkService Update started.");
        using var scope = ServiceProvider.CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<UpdateOnlyNameUniqueEntityCommand<BenchmarkEntity>>();
        var commandResult = await command.ExecuteAsync(model.MapToBenchmarkEntity());
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"BenchmarkService Update failed. {commandResult.ErrorMessage}");

            return new CommandResult(commandResult.ErrorMessage);
        }

        Logger.Info("BenchmarkService Update successfully completed.");

        return new CommandResult();
    }

    /// <inheritdoc/>
    public async Task<CommandResult> RemoveAsync(IEnumerable<BenchmarkModel> model)
    {
        Logger.Info("BenchmarkService Remove started.");
        using var scope = ServiceProvider.CreateScope();
        var command = scope.ServiceProvider.GetRequiredService<RemoveEntitiesCommand<BenchmarkEntity>>();
        var commandResult = await command.ExecuteAsync(model.Select(b => b.MapToBenchmarkEntity()));
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"BenchmarkService Remove failed. {commandResult.ErrorMessage}");

            return new CommandResult(commandResult.ErrorMessage);
        }

        Logger.Info("BenchmarkService Remove successfully completed.");

        return new CommandResult();
    }
}
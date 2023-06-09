using CleanModels;
using CleanModels.Benchmark;
using CleanModels.Commands.Base;
using CleanModels.Queries.Base;
using Client.Clients.Base;
using Client.Services;
using Client.Services.Mappers.Csv;
using Client.Services.Parsers;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using static BenchmarkService.Benchmark;
using Benchmark = CleanModels.Benchmark.Benchmark;

namespace Cli.Models;

public class BenchmarkModel
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="BenchmarkModel"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    public BenchmarkModel(IServiceProvider serviceProvider, IEntityServiceClient<BenchmarkClient, Benchmark> serviceClient)
    {
        ServiceProvider = serviceProvider;
        ServiceClient = serviceClient;
    }

    private IServiceProvider ServiceProvider { get; }

    private IEntityServiceClient<BenchmarkClient, Benchmark> ServiceClient { get; }

    public async Task<QueryResult<List<Benchmark>>> GetBenchmarkAsync(PageModel pageModel)
    {
        Logger.Info("GetBenchmark started.");
        var queryResult = await ServiceClient.GetAsync(pageModel);
        if (!queryResult.IsSuccessful)
        {
            return new QueryResult<List<Benchmark>>(queryResult.ErrorMessage);
        }

        Logger.Info("GetBenchmark successfully completed.");

        return new QueryResult<List<Benchmark>>(queryResult.Data);
    }

    public async Task<CommandResult> AddOrUpdateBenchmarkAsync(Benchmark model)
    {
        Logger.Info("AddOrUpdateBenchmark started.");
        var commandResult = await ServiceClient.AddOrUpdateAsync(model);
        if (!commandResult.IsSuccessful)
        {
            return new CommandResult(commandResult.ErrorMessage);
        }

        Logger.Info("AddOrUpdateBenchmark successfully completed.");

        return new CommandResult();
    }

    public async Task<CommandResult> GetFromFileAsync(string path) // TODO rename
    {
        // var fileService = ServiceProvider.GetRequiredService<FileService>();
        // byte[] bytes = await fileService.GetFileBytesAsync(path);
        using var scope = ServiceProvider.CreateScope();
        var fileParser = scope.ServiceProvider
            .GetRequiredService<FileParser<List<ConfigurationRecommendation?>, ConfigurationRecommendationMapper>>();

        var parseResult = await fileParser.Parse(path);
        if (!parseResult.IsSuccessful)
        {
            return new CommandResult("Err");
        }

        return new CommandResult();
    }
}
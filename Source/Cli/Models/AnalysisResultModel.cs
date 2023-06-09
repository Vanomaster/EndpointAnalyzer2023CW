using CleanModels;
using CleanModels.Queries.Base;
using Client.Clients.Base;
using NLog;
using static BenchmarkService.Benchmark;
using Benchmark = CleanModels.Benchmark.Benchmark;

namespace Cli.Models;

public class AnalysisResultModel
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="AnalysisResultModel"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    public AnalysisResultModel(
        IServiceProvider serviceProvider,
        IEntityServiceClient<BenchmarkClient,
        Benchmark> serviceClient)
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

        if (string.IsNullOrEmpty(queryResult.Data))
        {
            queryResult.Data = "Ошибка при выполнении анализа. Обратитесь в поддержку.";
        }

        Logger.Info("GetBenchmark successfully completed.");

        return new QueryResult<List<Benchmark>>(queryResult.Data);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanModels.Analysis;
using CleanModels.Network;
using CleanModels.Queries.Base;
using Client.Clients;
using Client.Clients.Base;
using Gui.Common;
using NLog;
using static BenchmarkService.Benchmark;
using static NetworkService.Network;
using AnalysisResultModel = CleanModels.Analysis.AnalysisResult;
using Benchmark = CleanModels.Benchmark.Benchmark;

namespace Gui.Models;

public class HomeModel
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeModel"/> class.
    /// </summary>
    /// <param name="networkServiceClient">NetworkServiceClient.</param>
    /// <param name="serviceClient">.</param>
    public HomeModel(
        IReadOnlyServiceClient<NetworkClient, Host> networkServiceClient,
        IEntityServiceClient<BenchmarkClient, Benchmark> benchmarkServiceClient,
        AnalysisServiceClient serviceClient)
    {
        NetworkServiceClient = networkServiceClient;
        BenchmarkServiceClient = benchmarkServiceClient;
        ServiceClient = serviceClient;
    }

    private IReadOnlyServiceClient<NetworkClient, Host> NetworkServiceClient { get; }

    private IEntityServiceClient<BenchmarkClient, Benchmark> BenchmarkServiceClient { get; }

    private AnalysisServiceClient ServiceClient { get; }

    public async Task<QueryResult<List<Host>>> GetHostsAsync()
    {
        Logger.Info("HomeModel GetHosts started.");
        var commandResult = await NetworkServiceClient.GetAsync();
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"HomeModel GetHosts failed. {commandResult.ErrorMessage}");

            return new QueryResult<List<Host>>(TextConstants.GetHostsError);
        }

        Logger.Info("HomeModel GetHosts successfully completed.");

        return new QueryResult<List<Host>>(commandResult.Data);
    }

    public async Task<QueryResult<List<string>>> GetAllBenchmarkNamesAsync()
    {
        Logger.Info("HomeModel GetHosts started.");
        var commandResult = await BenchmarkServiceClient.GetAllNamesAsync();
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"HomeModel GetHosts failed. {commandResult.ErrorMessage}");

            return new QueryResult<List<string>>(TextConstants.GetHostsError);
        }

        Logger.Info("HomeModel GetHosts successfully completed.");

        return new QueryResult<List<string>>(commandResult.Data);
    }

    public async Task<QueryResult<string>> AnalyzeAsync(AnalysisModel analysisModel)
    {
        Logger.Info("HomeModel Analyze started.");
        var commandResult = await ServiceClient.AnalyzeAsync(analysisModel);
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"HomeModel Analyze failed. {commandResult.ErrorMessage}");

            return new QueryResult<string>(TextConstants.HomeModelAnalyzeError);
        }

        Logger.Info("HomeModel Analyze successfully completed.");

        return new QueryResult<string>(data: TextConstants.HomeModelAnalyzeSuccess);
    }
}
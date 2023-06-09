using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanModels;
using CleanModels.Commands.Base;
using CleanModels.Network;
using CleanModels.Queries.Base;
using CleanModels.Schedule;
using Client.Clients;
using Client.Clients.Base;
using Gui.Common;
using NLog;
using static BenchmarkService.Benchmark;
using static NetworkService.Network;
using Benchmark = CleanModels.Benchmark.Benchmark;

namespace Gui.Models;

public class SchedulerModel
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="SchedulerModel"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    /// <param name="serviceClient">ScheduleServiceClient.</param>
    /// <param name="networkServiceClient">NetworkServiceClient.</param>
    public SchedulerModel(
        IServiceProvider serviceProvider,
        ScheduleServiceClient serviceClient,
        IReadOnlyServiceClient<NetworkClient, Host> networkServiceClient,
        IEntityServiceClient<BenchmarkClient, Benchmark> benchmarkServiceClient)
    {
        ServiceProvider = serviceProvider;
        ServiceClient = serviceClient;
        NetworkServiceClient = networkServiceClient;
        BenchmarkServiceClient = benchmarkServiceClient;
    }

    private IServiceProvider ServiceProvider { get; }

    private ScheduleServiceClient ServiceClient { get; }

    private IReadOnlyServiceClient<NetworkClient, Host> NetworkServiceClient { get; }

    private IEntityServiceClient<BenchmarkClient, Benchmark> BenchmarkServiceClient { get; }

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

    public async Task<QueryResult<List<AnalysisScheduleRecord>>> GetAsync(PageModel pageModel)
    {
        Logger.Info("SchedulerModel Get started.");
        var queryResult = await ServiceClient.GetAsync(pageModel);
        if (!queryResult.IsSuccessful)
        {
            Logger.Error($"SchedulerModel Get failed. {queryResult.ErrorMessage}");

            return new QueryResult<List<AnalysisScheduleRecord>>(TextConstants.SchedulerModelGetError);
        }

        Logger.Info("SchedulerModel Get successfully completed.");

        return new QueryResult<List<AnalysisScheduleRecord>>(queryResult.Data);
    }

    public async Task<CommandResult> AddOrUpdateAsync(AnalysisScheduleRecord model)
    {
        Logger.Info("SchedulerModel AddOrUpdate started.");
        var commandResult = await ServiceClient.AddOrUpdateAsync(model);
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"SchedulerModel AddOrUpdate failed. {commandResult.ErrorMessage}");

            return new CommandResult(TextConstants.SchedulerModelAddOrUpdateError);
        }

        Logger.Info("SchedulerModel AddOrUpdate successfully completed.");

        return new CommandResult();
    }

    public async Task<CommandResult> RunAsync(string name)
    {
        Logger.Info("SchedulerModel Run started.");
        var commandResult = await ServiceClient.RunAsync(name);
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"SchedulerModel Run failed. {commandResult.ErrorMessage}");

            return new CommandResult(TextConstants.SchedulerModelRunError);
        }

        Logger.Info("SchedulerModel Run successfully completed.");

        return new CommandResult();
    }

    public async Task<CommandResult> RemoveAsync(IEnumerable<AnalysisScheduleRecord> model)
    {
        Logger.Info("SchedulerModel Remove started.");
        var commandResult = await ServiceClient.RemoveAsync(model);
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"SchedulerModel Remove failed. {commandResult.ErrorMessage}");

            return new CommandResult(commandResult.ErrorMessage + "\n" +
                                     TextConstants.BenchmarksModelRemoveError);
        }

        Logger.Info("SchedulerModel Remove successfully completed.");

        return new CommandResult();
    }
}
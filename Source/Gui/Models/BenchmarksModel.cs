using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanModels;
using CleanModels.Commands.Base;
using CleanModels.Queries.Base;
using Client.Clients.Base;
using Gui.Common;
using NLog;
using static BenchmarkService.Benchmark;
using Benchmark = CleanModels.Benchmark.Benchmark;

namespace Gui.Models;

public class BenchmarksModel
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="BenchmarksModel"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    /// <param name="serviceClient">ServiceClient.</param>
    public BenchmarksModel(
        IServiceProvider serviceProvider,
        IEntityServiceClient<BenchmarkClient, Benchmark> serviceClient)
    {
        ServiceProvider = serviceProvider;
        ServiceClient = serviceClient;
    }

    private IServiceProvider ServiceProvider { get; }

    private IEntityServiceClient<BenchmarkClient, Benchmark> ServiceClient { get; }

    public async Task<QueryResult<List<Benchmark>>> GetAsync(PageModel pageModel)
    {
        Logger.Info("BenchmarksModel Get started.");
        var queryResult = await ServiceClient.GetAsync(pageModel);
        if (!queryResult.IsSuccessful)
        {
            Logger.Error($"BenchmarksModel Get failed. {queryResult.ErrorMessage}");

            return new QueryResult<List<Benchmark>>(queryResult.ErrorMessage + "\n" +
                                                    TextConstants.BenchmarksModelGetError);
        }

        Logger.Info("BenchmarksModel Get successfully completed.");

        return new QueryResult<List<Benchmark>>(queryResult.Data);
    }

    public async Task<CommandResult> AddAsync(Benchmark model)
    {
        Logger.Info("BenchmarksModel Add started.");
        var commandResult = await ServiceClient.AddAsync(model);
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"BenchmarksModel Add failed. {commandResult.ErrorMessage}");

            return new CommandResult(commandResult.ErrorMessage + "\n" +
                                     TextConstants.BenchmarksModelAddError);
        }

        Logger.Info("BenchmarksModel Add successfully completed.");

        return new CommandResult();
    }

    public async Task<CommandResult> UpdateAsync(Benchmark model)
    {
        Logger.Info("BenchmarksModel Update started.");
        var commandResult = await ServiceClient.UpdateAsync(model);
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"BenchmarksModel Update failed. {commandResult.ErrorMessage}");

            return new CommandResult(commandResult.ErrorMessage + "\n" +
                                     TextConstants.BenchmarksModelUpdateError);
        }

        Logger.Info("BenchmarksModel Update successfully completed.");

        return new CommandResult();
    }

    public async Task<CommandResult> RemoveAsync(IEnumerable<Benchmark> model)
    {
        Logger.Info("BenchmarksModel Remove started.");
        var commandResult = await ServiceClient.RemoveAsync(model);
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"BenchmarksModel Remove failed. {commandResult.ErrorMessage}");

            return new CommandResult(commandResult.ErrorMessage + "\n" +
                                     TextConstants.BenchmarksModelRemoveError);
        }

        Logger.Info("BenchmarksModel Remove successfully completed.");

        return new CommandResult();
    }
}
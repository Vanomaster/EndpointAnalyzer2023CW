﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CleanModels;
using CleanModels.Benchmark;
using CleanModels.Commands.Base;
using CleanModels.Queries.Base;
using Client.Clients;
using Gui.Common;
using NLog;
using TrustedHardwareBenchmarkModel = CleanModels.Benchmark.TrustedHardwareBenchmark;

namespace Gui.Models;

public class TrustHardBenchmarksModel
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="TrustHardBenchmarksModel"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    /// <param name="serviceClient">ServiceClient.</param>
    /// <param name="fileParser">FileParser.</param>s
    /// <param name="configurationRecommendationsBenchmarkServiceClient">.</param>
    public TrustHardBenchmarksModel(
        //IParser<ConfigurationRecommendation, ConfigurationRecommendationMapper> fileParser,
        TrustHardBenchmarkServiceClient configurationRecommendationsBenchmarkServiceClient,
        TrustHardServiceClient recsServiceClient)
    {
        //FileParser = fileParser;
        ServiceClient = configurationRecommendationsBenchmarkServiceClient;
        RecsServiceClient = recsServiceClient;
    }

    //private IParser<ConfigurationRecommendation, ConfigurationRecommendationMapper> FileParser { get; }

    private TrustHardBenchmarkServiceClient ServiceClient { get; }

    private TrustHardServiceClient RecsServiceClient { get; }

    public async Task<QueryResult<TrustedHardwareBenchmarkModel>> GetAsync(string parentName)
    {
        Logger.Info("ConfigurationRecommendationsBenchmarksModel Get started.");
        var queryResult = await ServiceClient.GetAsync(parentName);
        if (!queryResult.IsSuccessful)
        {
            Logger.Error($"ConfigurationRecommendationsBenchmarksModel Get failed. {queryResult.ErrorMessage}");

            return new QueryResult<TrustedHardwareBenchmarkModel>(queryResult.ErrorMessage + "\n" + TextConstants.BenchmarksModelGetError);
        }

        Logger.Info("ConfigurationRecommendationsBenchmarksModel Get successfully completed.");

        return new QueryResult<TrustedHardwareBenchmarkModel>(queryResult.Data);
    }

    // public async Task<QueryResult<List<string>>> GetAllNamesAsync()
    // {
    //     Logger.Info("ConfigurationRecommendationsBenchmarksModel Get started.");
    //     var queryResult = await ServiceClient.GetAllNamesAsync();
    //     if (!queryResult.IsSuccessful)
    //     {
    //         Logger.Error($"ConfigurationRecommendationsBenchmarksModel Get failed. {queryResult.ErrorMessage}");
    //
    //         return new QueryResult<List<string>>(TextConstants.BenchmarksModelGetError);
    //     }
    //
    //     Logger.Info("ConfigurationRecommendationsBenchmarksModel Get successfully completed.");
    //
    //     return new QueryResult<List<string>>(queryResult.Data);
    // }

    public async Task<CommandResult> AddAsync(TrustedHardwareBenchmarkModel model)
    {
        Logger.Info("ConfigurationRecommendationsBenchmarksModel Add started.");
        var commandResult = await ServiceClient.AddAsync(model);
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"ConfigurationRecommendationsBenchmarksModel Add failed. {commandResult.ErrorMessage}");

            return new CommandResult(commandResult.ErrorMessage + "\n" + TextConstants.BenchmarksModelAddError);
        }

        Logger.Info("ConfigurationRecommendationsBenchmarksModel Add successfully completed.");

        return new CommandResult();
    }

    // public async Task<CommandResult> UpdateAsync(ConfigurationRecommendationsBenchmark model)
    // {
    //     Logger.Info("ConfigurationRecommendationsBenchmarksModel Update started.");
    //     var commandResult = await ServiceClient.UpdateAsync(model);
    //     if (!commandResult.IsSuccessful)
    //     {
    //         Logger.Error($"ConfigurationRecommendationsBenchmarksModel Update failed. {commandResult.ErrorMessage}");
    //
    //         return new CommandResult(TextConstants.BenchmarksModelUpdateError);
    //     }
    //
    //     Logger.Info("ConfigurationRecommendationsBenchmarksModel Update successfully completed.");
    //
    //     return new CommandResult();
    // }

    // public async Task<CommandResult> RemoveAsync(ConfigurationRecommendationsBenchmark model)
    // {
    //     Logger.Info("ConfigurationRecommendationsBenchmarksModel Remove started.");
    //     var commandResult = await ServiceClient.RemoveAsync(model);
    //     if (!commandResult.IsSuccessful)
    //     {
    //         Logger.Error($"ConfigurationRecommendationsBenchmarksModel Remove failed. {commandResult.ErrorMessage}");
    //
    //         return new CommandResult(TextConstants.BenchmarksModelRemoveError);
    //     }
    //
    //     Logger.Info("ConfigurationRecommendationsBenchmarksModel Remove successfully completed.");
    //
    //     return new CommandResult();
    // }

    public async Task<QueryResult<List<TrustedHardware>>> GetAsync(PageModel pageModel)
    {
        Logger.Info("ConfigurationRecommendationsBenchmarksModel Add started.");
        var queryResult = await RecsServiceClient.GetAsync(pageModel);
        if (!queryResult.IsSuccessful)
        {
            Logger.Error($"ConfigurationRecommendationsBenchmarksModel Add failed. {queryResult.ErrorMessage}");

            return new QueryResult<List<TrustedHardware>>(queryResult.ErrorMessage + "\n" + TextConstants.BenchmarkModelGetItemsError);
        }

        Logger.Info("ConfigurationRecommendationsBenchmarksModel Add successfully completed.");

        return new QueryResult<List<TrustedHardware>>(queryResult.Data);
    }

    public async Task<CommandResult> AddAsync(IEnumerable<TrustedHardware> model)
    {
        Logger.Info("ConfigurationRecommendationsBenchmarksModel Add started.");
        var commandResult = await RecsServiceClient.AddAsync(model);
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"ConfigurationRecommendationsBenchmarksModel Add failed. {commandResult.ErrorMessage}");

            return new CommandResult(commandResult.ErrorMessage + "\n" + TextConstants.BenchmarksModelAddError);
        }

        Logger.Info("ConfigurationRecommendationsBenchmarksModel Add successfully completed.");

        return new CommandResult();
    }

    // public async Task<CommandResult> UpdateAsync(IEnumerable<ConfigurationRecommendation> model)
    // {
    //     Logger.Info("ConfigurationRecommendationsBenchmarksModel Update started.");
    //     var commandResult = await RecsServiceClient.UpdateAsync(model);
    //     if (!commandResult.IsSuccessful)
    //     {
    //         Logger.Error($"ConfigurationRecommendationsBenchmarksModel Update failed. {commandResult.ErrorMessage}");
    //
    //         return new CommandResult(TextConstants.BenchmarksModelUpdateError);
    //     }
    //
    //     Logger.Info("ConfigurationRecommendationsBenchmarksModel Update successfully completed.");
    //
    //     return new CommandResult();
    // }

    public async Task<CommandResult> RemoveAsync(IEnumerable<TrustedHardware> model)
    {
        Logger.Info("ConfigurationRecommendationsBenchmarksModel Remove started.");
        var commandResult = await RecsServiceClient.RemoveAsync(model);
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"ConfigurationRecommendationsBenchmarksModel Remove failed. {commandResult.ErrorMessage}");

            return new CommandResult(commandResult.ErrorMessage + "\n" + TextConstants.BenchmarksModelRemoveError);
        }

        Logger.Info("ConfigurationRecommendationsBenchmarksModel Remove successfully completed.");

        return new CommandResult();
    }

    // public QueryResult<List<ConfigurationRecommendation>> GetFromFile(string filePath)
    // {
    //     Logger.Info("ConfigurationRecommendationsBenchmarksModel GetFromFile started.");
    //     using var scope = ServiceProvider.CreateScope();
    //     var parseResult = FileParser.Parse(filePath);
    //     if (!parseResult.IsSuccessful)
    //     {
    //         Logger.Error($"ConfigurationRecommendationsBenchmarksModel GetFromFile failed. {parseResult.ErrorMessage}");
    //
    //         return new QueryResult<List<ConfigurationRecommendation>>(Constants.BenchmarksModelGetFromFileError);
    //     }
    //
    //     Logger.Info("ConfigurationRecommendationsBenchmarksModel GetFromFile successfully completed.");
    //
    //     return new QueryResult<List<ConfigurationRecommendation>>(parseResult.Data);
    // }
}
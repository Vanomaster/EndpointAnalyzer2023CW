using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanModels;
using CleanModels.Queries.Base;
using Client.Clients.Base;
using Gui.Common;
using NLog;
using static AnalysisResultService.AnalysisResult;
using AnalysisResultModel = CleanModels.Analysis.AnalysisResult;

namespace Gui.Models;

public class AnalysisResultsModel
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="AnalysisResultsModel"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    /// <param name="serviceClient">.</param>
    public AnalysisResultsModel(
        IServiceProvider serviceProvider,
        IReadOnlyServiceClient<AnalysisResultClient, PageModel, AnalysisResultModel> serviceClient)
    {
        ServiceProvider = serviceProvider;
        ServiceClient = serviceClient;
    }

    private IServiceProvider ServiceProvider { get; }

    private IReadOnlyServiceClient<AnalysisResultClient, PageModel, AnalysisResultModel> ServiceClient { get; }

    public async Task<QueryResult<List<AnalysisResultModel>>> GetAsync(PageModel pageModel)
    {
        Logger.Info("AnalysisResultsModel Get started.");
        var queryResult = await ServiceClient.GetAsync(pageModel);
        if (!queryResult.IsSuccessful)
        {
            Logger.Error($"AnalysisResultsModel Get failed. {queryResult.ErrorMessage}");

            return new QueryResult<List<AnalysisResultModel>>(TextConstants.AnalysisResultsModelGetError);
        }

        Logger.Info("AnalysisResultsModel Get successfully completed.");

        return new QueryResult<List<AnalysisResultModel>>(queryResult.Data);
    }
}
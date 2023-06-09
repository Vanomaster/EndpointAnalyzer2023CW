using CleanModels;
using CleanModels.Queries.Base;
using Dal.Entities;
using Dal.Queries;
using NLog;
using Server.Services.Base;

namespace Server.Services;

public class AnalysisResultService : IAnalysisResultService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="AnalysisResultService"/> class.
    /// </summary>
    /// <param name="analysisResultQuery">AnalysisScheduleRecordPcNameQuery.</param>
    public AnalysisResultService(AnalysisResultQuery analysisResultQuery)
    {
        AnalysisResultQuery = analysisResultQuery;
    }

    private AnalysisResultQuery AnalysisResultQuery { get; }

    public async Task<QueryResult<List<AnalysisResult>>> Get(PageModel pageModel)
    {
        Logger.Info("AnalysisResultService Get started.");
        var queryResult = await AnalysisResultQuery.ExecuteAsync(pageModel);
        if (!queryResult.IsSuccessful)
        {
            Logger.Error($"AnalysisResultService Get failed. {queryResult.ErrorMessage}");

            return new QueryResult<List<AnalysisResult>>(queryResult.ErrorMessage);
        }

        Logger.Info("AnalysisResultService Get successfully completed.");

        return new QueryResult<List<AnalysisResult>>(queryResult.Data);
    }
}
using Analyzers.Base;
using CleanModels.Analysis;
using CleanModels.Commands.Base;
using CleanModels.Queries.Base;
using Dal.Commands;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Server.Services.Base;
using AnalysisResult = Dal.Entities.AnalysisResult;

namespace Server.Services;

public class AnalysisService : IAnalysisService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="AnalysisService"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    public AnalysisService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    private IServiceProvider ServiceProvider { get; }

    public async Task<CommandResult> AnalyzeAsync(AnalysisModel analysisModel)
    {
        Logger.Info("AnalysisService Analyze started.");
        foreach (string analyzerName in analysisModel.AnalyzerNames)
        {
            var analysisStartDateTime = DateTime.Now;
            var analyzerQueryResult = GetAnalyzer(analyzerName);
            if (!analyzerQueryResult.IsSuccessful)
            {
                Logger.Error($"AnalysisService Analyze failed. {analyzerQueryResult.ErrorMessage}");

                return new CommandResult(analyzerQueryResult.ErrorMessage);
            }

            var analyzer = analyzerQueryResult.Data;
            var queryResult = await analyzer.AnalyzeAsync(analysisModel);
            if (!queryResult.IsSuccessful)
            {
                Logger.Error($"AnalysisService Analyze failed. {queryResult.ErrorMessage}");

                return new CommandResult(queryResult.ErrorMessage);
            }

            using var scope = ServiceProvider.CreateScope();
            var networkService = scope.ServiceProvider.GetRequiredService<INetworkService>();
            var hostWithEaServiceQueryResult = networkService.GetHostWithEaService(analysisModel.PcIp);
            if (!hostWithEaServiceQueryResult.IsSuccessful)
            {
                Logger.Error($"AnalysisService Analyze failed. {hostWithEaServiceQueryResult.ErrorMessage}");
            }

            var analysisResult = new AnalysisResult
            {
                PcName = hostWithEaServiceQueryResult.Data?.Name ?? analysisModel.PcIp,
                BenchmarkName = analysisModel.BenchmarkName,
                AnalyzerName = analyzerName,
                Text = queryResult.Data,
                DateTime = analysisStartDateTime,
            };

            var command = scope.ServiceProvider.GetRequiredService<AddOrUpdateEntityCommand<AnalysisResult>>();
            var commandResult = await command.ExecuteAsync(new[] { analysisResult });
            if (!commandResult.IsSuccessful)
            {
                Logger.Error($"AnalysisService Analyze failed. {commandResult.ErrorMessage}");

                return new CommandResult(commandResult.ErrorMessage);
            }
        }

        Logger.Info("AnalysisService Analyze successfully completed.");

        return new CommandResult();
    }

    private QueryResult<IAnalyzer<AnalysisModel, byte[]>> GetAnalyzer(string analyzerName)
    {
        using var scope = ServiceProvider.CreateScope();
        var analyzerTypeQueryResult = AnalyzerProvider.GetAnalyzerTypeByName(analyzerName);
        if (!analyzerTypeQueryResult.IsSuccessful)
        {
            return new QueryResult<IAnalyzer<AnalysisModel, byte[]>>(analyzerTypeQueryResult.ErrorMessage);
        }

        var analyzerType = analyzerTypeQueryResult.Data;
        if (scope.ServiceProvider.GetRequiredService(analyzerType) is not IAnalyzer<AnalysisModel, byte[]> analyzer)
        {
            return new QueryResult<IAnalyzer<AnalysisModel, byte[]>>(
                $"Service type {analyzerType} is not {typeof(IAnalyzer<AnalysisModel, byte[]>)}");
        }

        return new QueryResult<IAnalyzer<AnalysisModel, byte[]>>(analyzer);

    }
}
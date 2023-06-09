using AnalysisService;
using CommonService;
using Grpc.Core;
using NLog;
using Server.Services.Base;

namespace Server.Controllers;

/// <inheritdoc />
public class AnalysisServiceController : Analysis.AnalysisBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnalysisServiceController"/> class.
    /// </summary>
    /// <param name="analysisService">Benchmark service.</param>
    public AnalysisServiceController(IAnalysisService analysisService)
    {
        AnalysisService = analysisService;
    }

    private IAnalysisService AnalysisService { get; }

    /// <inheritdoc/>
    public override async Task<CommandResponse> Analyze(AnalyzeRequest model, ServerCallContext context)
    {
        var response = await AnalyzeAsync(model);

        return response;
    }

    private async Task<CommandResponse> AnalyzeAsync(AnalyzeRequest model)
    {
        var commandResult = await AnalysisService.AnalyzeAsync(model.AnalysisModel.MapToAnalysisModel());
        if (!commandResult.IsSuccessful)
        {
            return new CommandResponse { ErrorMessage = commandResult.ErrorMessage };
        }

        return new CommandResponse();
    }
}
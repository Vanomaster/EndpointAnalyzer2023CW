using AnalysisResultService;
using Grpc.Core;
using NLog;
using Server.Services.Base;

namespace Server.Controllers;

/// <inheritdoc />
public class AnalysisResultServiceController : AnalysisResult.AnalysisResultBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="AnalysisResultServiceController"/> class.
    /// </summary>
    /// <param name="analysisService">Benchmark service.</param>
    public AnalysisResultServiceController(IAnalysisResultService analysisService)
    {
        Logger.Debug("Creating AnalysisServiceController.");
        AnalysisResultService = analysisService;
    }

    private IAnalysisResultService AnalysisResultService { get; }

    /// <inheritdoc/>
    public override async Task<GetResponse> Get(GetRequest model, ServerCallContext context)
    {
        var response = await GetAsync(model);

        return response;
    }

    private async Task<GetResponse> GetAsync(GetRequest model)
    {
        var queryResult = await AnalysisResultService.Get(model.PageModel.MapToPageModel());
        if (!queryResult.IsSuccessful)
        {
            return new GetResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        var protoAnalysisResults = queryResult.Data.Select(analysisResult => analysisResult.MapToProtoAnalysisResult());
        var data = new Data();
        data.AnalysisResults.AddRange(protoAnalysisResults);

        return new GetResponse { Data = data };
    }
}
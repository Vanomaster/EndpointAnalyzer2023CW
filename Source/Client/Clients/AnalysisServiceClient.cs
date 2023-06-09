using AnalysisService;
using CleanModels.Commands.Base;
using Client.Clients.Base;
using static AnalysisService.Analysis;
using AnalysisModel = CleanModels.Analysis.AnalysisModel;

namespace Client.Clients;

/// <inheritdoc />
public class AnalysisServiceClient : ServiceClientBase<AnalysisClient>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnalysisServiceClient"/> class.
    /// </summary>
    /// <param name="client">Client.</param>
    public AnalysisServiceClient(AnalysisClient client)
         : base(client)
    {
    }

    public async Task<CommandResult> AnalyzeAsync(AnalysisModel analysisModel)
    {
        var request = new AnalyzeRequest { AnalysisModel = analysisModel.MapToProtoAnalysisModel() };
        var response = await Client.AnalyzeAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new CommandResult(response.ErrorMessage);
        }

        return new CommandResult();
    }
}
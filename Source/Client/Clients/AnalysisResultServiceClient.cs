using AnalysisResultService;
using CleanModels;
using CleanModels.Queries.Base;
using Client.Clients.Base;
using static AnalysisResultService.AnalysisResult;
using AnalysisResultModel = CleanModels.Analysis.AnalysisResult;

namespace Client.Clients;

/// <inheritdoc />
public class AnalysisResultServiceClient
    : ReadOnlyServiceClientBase<AnalysisResultClient, PageModel, AnalysisResultModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnalysisServiceClient"/> class.
    /// </summary>
    /// <param name="client">Client.</param>
    public AnalysisResultServiceClient(AnalysisResultClient client)
         : base(client)
    {
    }

    /// <inheritdoc/>
    public override async Task<QueryResult<List<AnalysisResultModel>>> GetAsync(PageModel pageModel)
    {
        var request = new GetRequest { PageModel = pageModel.MapToProtoPageModel() };
        var response = await Client.GetAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new QueryResult<List<AnalysisResultModel>>(response.ErrorMessage);
        }

        var analysisResults = response.Data.AnalysisResults
            .Select(r => r.MapToAnalysisResult())
            .ToList();

        return new QueryResult<List<AnalysisResultModel>>(data: analysisResults);
    }
}
using CleanModels.Benchmark;
using CleanModels.Commands.Base;
using CleanModels.Queries.Base;
using Client.Clients.Base;
using TrustedSoftwareService;
using PageModel = CleanModels.PageModel;
using TrustedSoftware = TrustedSoftwareService.TrustedSoftware;
using TrustedSoftwareModel = CleanModels.Benchmark.TrustedSoftware;

namespace Client.Clients;

/// <inheritdoc />
public class TrustSoftServiceClient : ServiceClientBase<TrustedSoftware.TrustedSoftwareClient>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationRecommendationsServiceClient"/> class.
    /// </summary>
    /// <param name="client">Client.</param>
    public TrustSoftServiceClient(TrustedSoftware.TrustedSoftwareClient client)
         : base(client)
    {
    }

    /// <inheritdoc/>
    public async Task<QueryResult<List<TrustedSoftwareModel>>> GetAsync(PageModel pageModel)
    {
        var request = new GetRequest { PageModel = pageModel.MapToProtoPageModel() };
        var response = await Client.GetAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new QueryResult<List<TrustedSoftwareModel>>(response.ErrorMessage);
        }

        var benchmark = response.Data.Recommendations.MapToTrustedSoftware();

        return new QueryResult<List<TrustedSoftwareModel>>(benchmark.ToList());
    }

    // /// <inheritdoc/>
    // public async Task<QueryResult<List<ConfigurationRecommendation>>> GetExistAsync(
    //     IEnumerable<ConfigurationRecommendation> model)
    // {
    //     var request = new Request();
    //     request.Recommendations.AddRange(model.MapToProtoConfigurationRecommendations());
    //     var response = await Client.GetExistAsync(request);
    //     if (!string.IsNullOrEmpty(response.ErrorMessage))
    //     {
    //         return new QueryResult<List<ConfigurationRecommendation>>(response.ErrorMessage);
    //     }
    //
    //     var benchmark = response.Data.Recommendations.MapToConfigurationRecommendations();
    //
    //     return new QueryResult<List<ConfigurationRecommendation>>(benchmark.ToList());
    // }

    /// <inheritdoc/>
    public async Task<CommandResult> AddAsync(IEnumerable<TrustedSoftwareModel> model)
    {
        var request = new Request();
        request.Hardware.AddRange(model.MapToProtoTrustedSoftware());
        var response = await Client.AddAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new CommandResult(response.ErrorMessage);
        }

        return new CommandResult();
    }

    // /// <inheritdoc/>
    // public async Task<CommandResult> UpdateAsync(IEnumerable<ConfigurationRecommendation> model)
    // {
    //     var request = new Request();
    //     request.Recommendations.AddRange(model.MapToProtoConfigurationRecommendations());
    //     var response = await Client.UpdateAsync(request);
    //     if (!string.IsNullOrEmpty(response.ErrorMessage))
    //     {
    //         return new CommandResult(response.ErrorMessage);
    //     }
    //
    //     return new CommandResult();
    // }

    /// <inheritdoc/>
    public async Task<CommandResult> RemoveAsync(IEnumerable<TrustedSoftwareModel> model)
    {
        var request = new Request();
        request.Hardware.AddRange(model.MapToProtoTrustedSoftware());
        var response = await Client.RemoveAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new CommandResult(response.ErrorMessage);
        }

        return new CommandResult();
    }
}
using CleanModels.Benchmark;
using CleanModels.Commands.Base;
using CleanModels.Queries.Base;
using Client.Clients.Base;
using ConfigurationRecommendationsService;
using static ConfigurationRecommendationsService.ConfigurationRecommendations;
using PageModel = CleanModels.PageModel;

namespace Client.Clients;

/// <inheritdoc />
public class ConfigurationRecommendationsServiceClient : ServiceClientBase<ConfigurationRecommendationsClient>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationRecommendationsServiceClient"/> class.
    /// </summary>
    /// <param name="client">Client.</param>
    public ConfigurationRecommendationsServiceClient(ConfigurationRecommendationsClient client)
         : base(client)
    {
    }

    /// <inheritdoc/>
    public async Task<QueryResult<List<ConfigurationRecommendation>>> GetAsync(PageModel pageModel)
    {
        var request = new GetRequest { PageModel = pageModel.MapToProtoPageModel() };
        var response = await Client.GetAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new QueryResult<List<ConfigurationRecommendation>>(response.ErrorMessage);
        }

        var benchmark = response.Data.Recommendations.MapToConfigurationRecommendations();

        return new QueryResult<List<ConfigurationRecommendation>>(benchmark.ToList());
    }

    /// <inheritdoc/>
    public async Task<QueryResult<List<ConfigurationRecommendation>>> GetExistAsync(
        IEnumerable<ConfigurationRecommendation> model)
    {
        var request = new Request();
        request.Recommendations.AddRange(model.MapToProtoConfigurationRecommendations());
        var response = await Client.GetExistAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new QueryResult<List<ConfigurationRecommendation>>(response.ErrorMessage);
        }

        var benchmark = response.Data.Recommendations.MapToConfigurationRecommendations();

        return new QueryResult<List<ConfigurationRecommendation>>(benchmark.ToList());
    }

    /// <inheritdoc/>
    public async Task<CommandResult> AddAsync(IEnumerable<ConfigurationRecommendation> model)
    {
        var request = new Request();
        request.Recommendations.AddRange(model.MapToProtoConfigurationRecommendations());
        var response = await Client.AddAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new CommandResult(response.ErrorMessage);
        }

        return new CommandResult();
    }

    /// <inheritdoc/>
    public async Task<CommandResult> UpdateAsync(IEnumerable<ConfigurationRecommendation> model)
    {
        var request = new Request();
        request.Recommendations.AddRange(model.MapToProtoConfigurationRecommendations());
        var response = await Client.UpdateAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new CommandResult(response.ErrorMessage);
        }

        return new CommandResult();
    }

    /// <inheritdoc/>
    public async Task<CommandResult> RemoveAsync(IEnumerable<ConfigurationRecommendation> model)
    {
        var request = new Request();
        request.Recommendations.AddRange(model.MapToProtoConfigurationRecommendations());
        var response = await Client.RemoveAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new CommandResult(response.ErrorMessage);
        }

        return new CommandResult();
    }
}
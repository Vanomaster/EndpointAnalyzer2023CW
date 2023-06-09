using CleanModels.Commands.Base;
using CleanModels.Queries.Base;
using Client.Clients.Base;
using TrustedSoftwareBenchmarkService;
using static TrustedSoftwareBenchmarkService.TrustedSoftwareBenchmark;
using TrustedSoftwareBenchmarkModel = CleanModels.Benchmark.TrustedSoftwareBenchmark;

namespace Client.Clients;

/// <inheritdoc />
public class TrustSoftBenchmarkServiceClient :
    ServiceClientBase<TrustedSoftwareBenchmarkClient>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationRecommendationsBenchmarkServiceClient"/> class.
    /// </summary>
    /// <param name="client">Client.</param>
    public TrustSoftBenchmarkServiceClient(TrustedSoftwareBenchmarkClient client)
         : base(client)
    {
    }

    /// <inheritdoc/>
    public async Task<QueryResult<TrustedSoftwareBenchmarkModel>> GetAsync(string parentName)
    {
        var request = new GetOneRequest { ParentName = parentName };
        var response = await Client.GetAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new QueryResult<TrustedSoftwareBenchmarkModel>(response.ErrorMessage);
        }

        var benchmark = response.Data.Benchmark.MapToTrustedSoftwareBenchmark();

        return new QueryResult<TrustedSoftwareBenchmarkModel>(benchmark);
    }

    // /// <inheritdoc/>
    // public async Task<QueryResult<List<string>>> GetAllNamesAsync()
    // {
    //     var request = new GetAllNamesRequest();
    //     var response = await Client.GetAllAsync(request);
    //     if (!string.IsNullOrEmpty(response.ErrorMessage))
    //     {
    //         return new QueryResult<List<string>>(response.ErrorMessage);
    //     }
    //
    //     return new QueryResult<List<string>>(response.Data.Names.ToList());
    // }

    /// <inheritdoc/>
    public async Task<CommandResult> AddAsync(TrustedSoftwareBenchmarkModel model)
    {
        var request = new TrustedSoftwareBenchmarkRequest()
            { Benchmark = model.MapToProtoTrustedSoftwareBenchmark() };

        var response = await Client.AddAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new CommandResult(response.ErrorMessage);
        }

        return new CommandResult();
    }

    // /// <inheritdoc/>
    // public async Task<CommandResult> UpdateAsync(ConfigurationRecommendationsBenchmark model)
    // {
    //     var request = new ConfigurationRecommendationsBenchmarkRequest
    //         { Benchmark = model.MapToProtoConfigurationRecommendationsBenchmark() };
    //
    //     var response = await Client.UpdateAsync(request);
    //     if (!string.IsNullOrEmpty(response.ErrorMessage))
    //     {
    //         return new CommandResult(response.ErrorMessage);
    //     }
    //
    //     return new CommandResult();
    // }
    //
    // /// <inheritdoc/>
    // public async Task<CommandResult> RemoveAsync(ConfigurationRecommendationsBenchmark model)
    // {
    //     var request = new ConfigurationRecommendationsBenchmarkRequest
    //         { Benchmark = model.MapToProtoConfigurationRecommendationsBenchmark() };
    //
    //     var response = await Client.RemoveAsync(request);
    //     if (!string.IsNullOrEmpty(response.ErrorMessage))
    //     {
    //         return new CommandResult(response.ErrorMessage);
    //     }
    //
    //     return new CommandResult();
    // }
}
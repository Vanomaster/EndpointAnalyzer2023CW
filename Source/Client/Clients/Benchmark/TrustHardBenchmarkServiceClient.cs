using CleanModels.Commands.Base;
using CleanModels.Queries.Base;
using Client.Clients.Base;
using TrustedHardwareBenchmarkService;
using static TrustedHardwareBenchmarkService.TrustedHardwareBenchmark;
using GetOneRequest = TrustedHardwareBenchmarkService.GetOneRequest;
using TrustedHardwareBenchmarkModel = CleanModels.Benchmark.TrustedHardwareBenchmark;

namespace Client.Clients;

/// <inheritdoc />
public class TrustHardBenchmarkServiceClient :
    ServiceClientBase<TrustedHardwareBenchmarkClient>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationRecommendationsBenchmarkServiceClient"/> class.
    /// </summary>
    /// <param name="client">Client.</param>
    public TrustHardBenchmarkServiceClient(TrustedHardwareBenchmarkClient client)
         : base(client)
    {
    }

    /// <inheritdoc/>
    public async Task<QueryResult<TrustedHardwareBenchmarkModel>> GetAsync(string parentName)
    {
        var request = new GetOneRequest { ParentName = parentName };
        var response = await Client.GetAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new QueryResult<TrustedHardwareBenchmarkModel>(response.ErrorMessage);
        }

        var benchmark = response.Data.Benchmark.MapToTrustedHardwareBenchmark();

        return new QueryResult<TrustedHardwareBenchmarkModel>(benchmark);
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
    public async Task<CommandResult> AddAsync(TrustedHardwareBenchmarkModel model)
    {
        var request = new TrustedHardwareBenchmarkRequest()
            { Benchmark = model.MapToProtoTrustedHardwareBenchmark() };

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
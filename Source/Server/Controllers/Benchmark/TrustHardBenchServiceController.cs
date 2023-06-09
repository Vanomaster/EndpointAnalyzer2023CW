using CommonService;
using Dal.Entities;
using Grpc.Core;
using Server.Services;
using Server.Services.Base;
using TrustedHardwareBenchmarkService;
using static TrustedHardwareBenchmarkService.TrustedHardwareBenchmark;

namespace Server.Controllers;

/// <inheritdoc />
public class TrustHardBenchServiceController : TrustedHardwareBenchmarkBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationRecommendationsServiceController"/> class.
    /// </summary>
    /// <param name="service">Service.</param>
    public TrustHardBenchServiceController(TrustHardBenchmarkService service)
    {
        Service = service;
    }

    private TrustHardBenchmarkService Service { get; }

    /// <inheritdoc/>
    public override async Task<GetTrustedHardwareBenchmarkResponse> Get(GetOneRequest model, ServerCallContext context)
    {
        var response = await GetAsync(model);

        return response;
    }

    // /// <inheritdoc/>
    // public override async Task<GetResponse> GetExist(Request model, ServerCallContext context)
    // {
    //     throw new NotImplementedException();
    //     //var response = await GetExistAsync(model);
    //
    //     //return response;
    // }

    /// <inheritdoc/>
    public override async Task<CommandResponse> Add(TrustedHardwareBenchmarkRequest model, ServerCallContext context)
    {
        var response = await AddAsync(model);

        return response;
    }

    // /// <inheritdoc/>
    // public override async Task<CommandResponse> Update(Request model, ServerCallContext context)
    // {
    //     throw new NotImplementedException();
    //     // var response = await UpdateAsync(model);
    //     //
    //     // return response;
    // }

    // /// <inheritdoc/>
    // public override async Task<CommandResponse> Remove(TrustedHardwareBenchmarkRequest model, ServerCallContext context)
    // {
    //     var response = await RemoveAsync(model);
    //
    //     return response;
    // }

    private async Task<GetTrustedHardwareBenchmarkResponse> GetAsync(GetOneRequest model)
    {
        var queryResult = await Service.GetOneAsync(model.ParentName);
        if (!queryResult.IsSuccessful)
        {
            return new GetTrustedHardwareBenchmarkResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        var data = new TrustedHardwareBenchmarkData();
        var protoModels = queryResult.Data.MapToProtoTrustedHardwareBenchmark();
        data.Benchmark = protoModels;

        return new GetTrustedHardwareBenchmarkResponse { Data = data };
    }

    // private async Task<GetResponse> GetExistAsync(Request model)
    // {
    //     var queryResult = await Service.GetExistAsync(model.Recommendations.MapToRecommendations().ToList());
    //     if (!queryResult.IsSuccessful)
    //     {
    //         return new GetResponse { ErrorMessage = queryResult.ErrorMessage };
    //     }
    //
    //     var data = new ConfigurationRecommendationsData();
    //     data.Recommendations.AddRange(queryResult.Data.MapToProtoConfigurationRecommendations());
    //
    //     return new GetResponse { Data = data };
    // }

    private async Task<CommandResponse> AddAsync(TrustedHardwareBenchmarkRequest model)
    {
        var recommendations = model.Benchmark.MapToTrustedHardwareBenchmark();
        var queryResult = await Service.AddAsync(recommendations);
        if (!queryResult.IsSuccessful)
        {
            return new CommandResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        return new CommandResponse();
    }

    // private async Task<CommandResponse> UpdateAsync(Request model)
    // {
    //     var recommendations = model.Recommendations.MapToRecommendations().ToList();
    //     var queryResult = await Service.UpdateAsync(recommendations);
    //     if (!queryResult.IsSuccessful)
    //     {
    //         return new CommandResponse { ErrorMessage = queryResult.ErrorMessage };
    //     }
    //
    //     return new CommandResponse();
    // }

    // private async Task<CommandResponse> RemoveAsync(TrustedHardwareBenchmarkRequest model)
    // {
    //     var recommendations = model.Benchmark.MapToTrustedHardwareBenchmark();
    //     var queryResult = await Service.(recommendations);
    //     if (!queryResult.IsSuccessful)
    //     {
    //         return new CommandResponse { ErrorMessage = queryResult.ErrorMessage };
    //     }
    //
    //     return new CommandResponse();
    // }
}
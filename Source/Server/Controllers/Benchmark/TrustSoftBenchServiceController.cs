using CommonService;
using Dal.Entities;
using Grpc.Core;
using Server.Services;
using Server.Services.Base;
using TrustedSoftwareBenchmarkService;
using static TrustedSoftwareBenchmarkService.TrustedSoftwareBenchmark;

namespace Server.Controllers;

/// <inheritdoc />
public class TrustSoftBenchServiceController : TrustedSoftwareBenchmarkBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationRecommendationsServiceController"/> class.
    /// </summary>
    /// <param name="service">Service.</param>
    public TrustSoftBenchServiceController(TrustSoftBenchmarkService service)
    {
        Service = service;
    }

    private TrustSoftBenchmarkService Service { get; }

    /// <inheritdoc/>
    public override async Task<GetTrustedSoftwareBenchmarkResponse> Get(GetOneRequest model, ServerCallContext context)
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
    public override async Task<CommandResponse> Add(TrustedSoftwareBenchmarkRequest model, ServerCallContext context)
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

    private async Task<GetTrustedSoftwareBenchmarkResponse> GetAsync(GetOneRequest model)
    {
        var queryResult = await Service.GetOneAsync(model.ParentName);
        if (!queryResult.IsSuccessful)
        {
            return new GetTrustedSoftwareBenchmarkResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        var data = new TrustedSoftwareBenchmarkData();
        var protoModels = queryResult.Data.MapToProtoTrustedSoftwareBenchmark();
        data.Benchmark = protoModels;

        return new GetTrustedSoftwareBenchmarkResponse { Data = data };
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

    private async Task<CommandResponse> AddAsync(TrustedSoftwareBenchmarkRequest model)
    {
        var recommendations = model.Benchmark.MapToTrustedSoftwareBenchmark();
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
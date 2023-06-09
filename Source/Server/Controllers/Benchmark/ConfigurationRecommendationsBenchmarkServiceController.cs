using CommonService;
using ConfigurationRecommendationsBenchmarkService;
using Grpc.Core;
using Server.Services.Base;
using static ConfigurationRecommendationsBenchmarkService.ConfigurationRecommendationsBenchmark;
using BenchmarkEntity = Dal.Entities.ConfigurationRecommendationsBenchmark;

namespace Server.Controllers;

/// <inheritdoc />
public class ConfigurationRecommendationsBenchmarkServiceController : ConfigurationRecommendationsBenchmarkBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationRecommendationsBenchmarkServiceController"/> class.
    /// </summary>
    /// <param name="benchmarkService">Benchmark service.</param>
    public ConfigurationRecommendationsBenchmarkServiceController(IDbService<BenchmarkEntity> benchmarkService)
    {
        BenchmarkService = benchmarkService;
    }

    private IDbService<BenchmarkEntity> BenchmarkService { get; }

    /// <inheritdoc/>
    public override async Task<GetConfigurationRecommendationsBenchmarkResponse> Get(
        GetOneRequest model, ServerCallContext context)
    {
        var response = await GetOneAsync(model);

        return response;
    }

    /// <inheritdoc/>
    public override async Task<GetAllConfigurationRecommendationsBenchmarkNamesResponse> GetAll(
        GetAllNamesRequest model, ServerCallContext context)
    {
        var response = await GetAllNamesAsync();

        return response;
    }

    /// <inheritdoc/>
    public override async Task<CommandResponse> Add(
        ConfigurationRecommendationsBenchmarkRequest model, ServerCallContext context)
    {
        var response = await AddAsync(model);

        return response;
    }

    // /// <inheritdoc/>
    // public override async Task<CommandResponse> Update(
    //     ConfigurationRecommendationsBenchmarkRequest model, ServerCallContext context)
    // {
    //     var response = await UpdateAsync(model);
    //
    //     return response;
    // }

    /// <inheritdoc/>
    public override async Task<CommandResponse> Remove(
        ConfigurationRecommendationsBenchmarkRequest model, ServerCallContext context)
    {
        var response = await RemoveAsync(model);

        return response;
    }

    private async Task<GetConfigurationRecommendationsBenchmarkResponse> GetOneAsync(GetOneRequest model)
    {
        var queryResult = await BenchmarkService.GetOneAsync(model.ParentName);
        if (!queryResult.IsSuccessful)
        {
            return new GetConfigurationRecommendationsBenchmarkResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        var data = new ConfigurationRecommendationsBenchmarkData
            { Benchmark = queryResult.Data.MapToProtoConfigurationRecommendationsBenchmark() };

        return new GetConfigurationRecommendationsBenchmarkResponse { Data = data };
    }

    private async Task<GetAllConfigurationRecommendationsBenchmarkNamesResponse> GetAllNamesAsync()
    {
        var queryResult = await BenchmarkService.GetAllNamesAsync();
        if (!queryResult.IsSuccessful)
        {
            return new GetAllConfigurationRecommendationsBenchmarkNamesResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        var data = new Data();
        data.Names.AddRange(queryResult.Data);

        return new GetAllConfigurationRecommendationsBenchmarkNamesResponse { Data = data };
    }

    private async Task<CommandResponse> AddAsync(ConfigurationRecommendationsBenchmarkRequest model)
    {
        var benchmark = model.Benchmark.MapToConfigurationRecommendationsBenchmark();
        if (benchmark == null)
        {
            return new CommandResponse { ErrorMessage = "Benchmark is null." };
        }

        var queryResult = await BenchmarkService.AddAsync(benchmark);
        if (!queryResult.IsSuccessful)
        {
            return new CommandResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        return new CommandResponse();
    }

    private async Task<CommandResponse> UpdateAsync(ConfigurationRecommendationsBenchmarkRequest model)
    {
        var benchmark = model.Benchmark.MapToConfigurationRecommendationsBenchmark();
        if (benchmark == null)
        {
            return new CommandResponse { ErrorMessage = "Benchmark is null." };
        }

        var queryResult = await BenchmarkService.UpdateAsync(benchmark);
        if (!queryResult.IsSuccessful)
        {
            return new CommandResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        return new CommandResponse();
    }

    private async Task<CommandResponse> RemoveAsync(ConfigurationRecommendationsBenchmarkRequest model)
    {
        var benchmark = model.Benchmark.MapToConfigurationRecommendationsBenchmark();
        var queryResult = await BenchmarkService.RemoveAsync(new[] { benchmark });
        if (!queryResult.IsSuccessful)
        {
            return new CommandResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        return new CommandResponse();
    }
}
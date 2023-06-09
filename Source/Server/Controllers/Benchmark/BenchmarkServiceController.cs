using BenchmarkService;
using CommonService;
using Grpc.Core;
using NLog;
using Server.Services.Base;
using static BenchmarkService.Benchmark;
using BenchmarkModel = CleanModels.Benchmark.Benchmark;

namespace Server.Controllers;

/// <inheritdoc />
public class BenchmarkServiceController : BenchmarkBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="BenchmarkServiceController"/> class.
    /// </summary>
    /// <param name="benchmarkService">Benchmark service.</param>
    public BenchmarkServiceController(IDbService<BenchmarkModel> benchmarkService)
    {
        Logger.Info("Creating BenchmarkServiceController.");
        BenchmarkService = benchmarkService;
    }

    private IDbService<BenchmarkModel> BenchmarkService { get; }

    /// <inheritdoc/>
    public override async Task<GetResponse> Get(GetRequest model, ServerCallContext context)
    {
        var response = await GetAsync(model);

        return response;
    }

    /// <inheritdoc/>
    public override async Task<GetAllNamesResponse> GetAllNames(GetAllNamesRequest model, ServerCallContext context)
    {
        var response = await GetAllNamesAsync();

        return response;
    }

    /// <inheritdoc/>
    public override async Task<CommandResponse> Add(AddOrUpdateRequest model, ServerCallContext context)
    {
        var response = await AddAsync(model);

        return response;
    }

    // /// <inheritdoc/>
    // public override async Task<CommandResponse> Update(AddOrUpdateRequest model, ServerCallContext context)
    // {
    //     var response = await UpdateAsync(model);
    //
    //     return response;
    // }

    /// <inheritdoc/>
    public override async Task<CommandResponse> Remove(RemoveRequest model, ServerCallContext context)
    {
        var response = await RemoveAsync(model);

        return response;
    }

    private async Task<GetResponse> GetAsync(GetRequest model)
    {
        var queryResult = await BenchmarkService.GetPageAsync(model.PageModel.MapToPageModel());
        if (!queryResult.IsSuccessful)
        {
            return new GetResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        var data = new ProtoBenchmarkData();
        var protoBenchmarks = queryResult.Data.Select(benchmark => benchmark.MapToProtoBenchmark());
        data.Benchmarks.AddRange(protoBenchmarks);

        return new GetResponse { Data = data };
    }

    private async Task<GetAllNamesResponse> GetAllNamesAsync()
    {
        var queryResult = await BenchmarkService.GetAllNamesAsync();
        if (!queryResult.IsSuccessful)
        {
            return new GetAllNamesResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        var data = new Data();
        data.Names.AddRange(queryResult.Data);

        return new GetAllNamesResponse { Data = data };
    }

    private async Task<CommandResponse> AddAsync(AddOrUpdateRequest model)
    {
        var benchmark = model.Benchmark.MapToBenchmark();
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

    private async Task<CommandResponse> UpdateAsync(AddOrUpdateRequest model)
    {
        var benchmark = model.Benchmark.MapToBenchmark();
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

    private async Task<CommandResponse> RemoveAsync(RemoveRequest model)
    {
        var benchmarks = model.Benchmarks.Select(b => b.MapToBenchmark()).ToList();
        var queryResult = await BenchmarkService.RemoveAsync(benchmarks);
        if (!queryResult.IsSuccessful)
        {
            return new CommandResponse { ErrorMessage = queryResult.ErrorMessage };
        }

        return new CommandResponse();
    }
}
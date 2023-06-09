// using BenchmarkService;
// using CommonService;
// using Grpc.Core;
// using NLog;
// using Server.Services.Base;
// using BenchmarkEntity = Dal.Entities.TrustedSoftwareBenchmark;
//
// namespace Server.Controllers;
//
// /// <inheritdoc />
// public class TrustedSoftwareBenchmarkServiceController : Benchmark.BenchmarkBase
// {
//     private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
//
//     /// <summary>
//     /// Initializes a new instance of the <see cref="TrustedSoftwareBenchmarkServiceController"/> class.
//     /// </summary>
//     /// <param name="benchmarkService">Benchmark service.</param>
//     public TrustedSoftwareBenchmarkServiceController(IDbService<BenchmarkEntity> benchmarkService)
//     {
//         Logger.Debug("Creating BenchmarkServiceController.");
//         BenchmarkService = benchmarkService;
//     }
//
//     private IDbService<BenchmarkEntity> BenchmarkService { get; }
//
//     /// <inheritdoc/>
//     public override async Task<GetTrustedSoftwareBenchmarkResponse> GetTrustedSoftwareBenchmark(GetRequest model, ServerCallContext context)
//     {
//         var response = await GetAsync(model);
//
//         return response;
//     }
//
//     /// <inheritdoc/>
//     public override async Task<CommandResponse> AddTrustedSoftwareBenchmark(AddOrUpdateTrustedSoftwareBenchmarkRequest model, ServerCallContext context)
//     {
//         var response = await AddAsync(model);
//
//         return response;
//     }
//
//     /// <inheritdoc/>
//     public override async Task<CommandResponse> UpdateTrustedSoftwareBenchmark(AddOrUpdateTrustedSoftwareBenchmarkRequest model, ServerCallContext context)
//     {
//         var response = await UpdateAsync(model);
//
//         return response;
//     }
//
//     /// <inheritdoc/>
//     public override async Task<CommandResponse> RemoveTrustedSoftwareBenchmarks(RemoveTrustedSoftwareBenchmarksRequest model, ServerCallContext context)
//     {
//         var response = await RemoveAsync(model);
//
//         return response;
//     }
//
//     private async Task<GetTrustedSoftwareBenchmarkResponse> GetAsync(GetRequest model)
//     {
//         var queryResult = await BenchmarkService.GetAsync(model.PageModel.MapToPageModel());
//         if (!queryResult.IsSuccessful)
//         {
//             return new GetTrustedSoftwareBenchmarkResponse { ErrorMessage = queryResult.ErrorMessage };
//         }
//
//         var data = new TrustedSoftwareBenchmarkData();
//         var protoBenchmarks = queryResult.Data.Select(benchmark => benchmark.MapToProtoTrustedSoftwareBenchmark());
//         data.Benchmarks.AddRange(protoBenchmarks);
//
//         return new GetTrustedSoftwareBenchmarkResponse { Data = data };
//     }
//
//     private async Task<CommandResponse> AddAsync(AddOrUpdateTrustedSoftwareBenchmarkRequest model)
//     {
//         var benchmark = model.Benchmark.MapToTrustedSoftwareBenchmark();
//         if (benchmark == null)
//         {
//             return new CommandResponse { ErrorMessage = "Benchmark is null." };
//         }
//
//         var queryResult = await BenchmarkService.AddAsync(benchmark);
//         if (!queryResult.IsSuccessful)
//         {
//             return new CommandResponse { ErrorMessage = queryResult.ErrorMessage };
//         }
//
//         return new CommandResponse();
//     }
//
//     private async Task<CommandResponse> UpdateAsync(AddOrUpdateTrustedSoftwareBenchmarkRequest model)
//     {
//         var benchmark = model.Benchmark.MapToTrustedSoftwareBenchmark();
//         if (benchmark == null)
//         {
//             return new CommandResponse { ErrorMessage = "Benchmark is null." };
//         }
//
//         var queryResult = await BenchmarkService.UpdateAsync(benchmark);
//         if (!queryResult.IsSuccessful)
//         {
//             return new CommandResponse { ErrorMessage = queryResult.ErrorMessage };
//         }
//
//         return new CommandResponse();
//     }
//
//     private async Task<CommandResponse> RemoveAsync(RemoveTrustedSoftwareBenchmarksRequest model)
//     {
//         var benchmarks = model.Benchmarks.Select(b => b.MapToTrustedSoftwareBenchmark()).ToList();
//         var queryResult = await BenchmarkService.RemoveAsync(benchmarks);
//         if (!queryResult.IsSuccessful)
//         {
//             return new CommandResponse { ErrorMessage = queryResult.ErrorMessage };
//         }
//
//         return new CommandResponse();
//     }
// }
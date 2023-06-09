// using BenchmarkService;
// using CommonService;
// using Grpc.Core;
// using NLog;
// using Server.Services.Base;
// using BenchmarkEntity = Dal.Entities.TrustedHardwareBenchmark;
//
// namespace Server.Controllers;
//
// /// <inheritdoc />
// public class TrustedHardwareBenchmarkServiceController : Benchmark.BenchmarkBase
// {
//     private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
//
//     /// <summary>
//     /// Initializes a new instance of the <see cref="TrustedHardwareBenchmarkServiceController"/> class.
//     /// </summary>
//     /// <param name="benchmarkService">Benchmark service.</param>
//     public TrustedHardwareBenchmarkServiceController(IDbService<BenchmarkEntity> benchmarkService)
//     {
//         Logger.Debug("Creating BenchmarkServiceController.");
//         BenchmarkService = benchmarkService;
//     }
//
//     private IDbService<BenchmarkEntity> BenchmarkService { get; }
//
//     /// <inheritdoc/>
//     public override async Task<GetTrustedHardwareBenchmarkResponse> GetTrustedHardwareBenchmark(GetRequest model, ServerCallContext context)
//     {
//         var response = await GetAsync(model);
//
//         return response;
//     }
//
//     /// <inheritdoc/>
//     public override async Task<CommandResponse> AddTrustedHardwareBenchmark(AddOrUpdateTrustedHardwareBenchmarkRequest model, ServerCallContext context)
//     {
//         var response = await AddAsync(model);
//
//         return response;
//     }
//
//     /// <inheritdoc/>
//     public override async Task<CommandResponse> UpdateTrustedHardwareBenchmark(AddOrUpdateTrustedHardwareBenchmarkRequest model, ServerCallContext context)
//     {
//         var response = await UpdateAsync(model);
//
//         return response;
//     }
//
//     /// <inheritdoc/>
//     public override async Task<CommandResponse> RemoveTrustedHardwareBenchmarks(RemoveTrustedHardwareBenchmarksRequest model, ServerCallContext context)
//     {
//         var response = await RemoveAsync(model);
//
//         return response;
//     }
//
//     private async Task<GetTrustedHardwareBenchmarkResponse> GetAsync(GetRequest model)
//     {
//         var queryResult = await BenchmarkService.GetAsync(model.PageModel.MapToPageModel());
//         if (!queryResult.IsSuccessful)
//         {
//             return new GetTrustedHardwareBenchmarkResponse { ErrorMessage = queryResult.ErrorMessage };
//         }
//
//         var data = new TrustedHardwareBenchmarkData();
//         var protoBenchmarks = queryResult.Data.Select(benchmark => benchmark.MapToProtoTrustedHardwareBenchmark());
//         data.Benchmarks.AddRange(protoBenchmarks);
//
//         return new GetTrustedHardwareBenchmarkResponse { Data = data };
//     }
//
//     private async Task<CommandResponse> AddAsync(AddOrUpdateTrustedHardwareBenchmarkRequest model)
//     {
//         var benchmark = model.Benchmark.MapToTrustedHardwareBenchmark();
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
//     private async Task<CommandResponse> UpdateAsync(AddOrUpdateTrustedHardwareBenchmarkRequest model)
//     {
//         var benchmark = model.Benchmark.MapToTrustedHardwareBenchmark();
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
//     private async Task<CommandResponse> RemoveAsync(RemoveTrustedHardwareBenchmarksRequest model)
//     {
//         var benchmarks = model.Benchmarks.Select(b => b.MapToTrustedHardwareBenchmark()).ToList();
//         var queryResult = await BenchmarkService.RemoveAsync(benchmarks);
//         if (!queryResult.IsSuccessful)
//         {
//             return new CommandResponse { ErrorMessage = queryResult.ErrorMessage };
//         }
//
//         return new CommandResponse();
//     }
// }
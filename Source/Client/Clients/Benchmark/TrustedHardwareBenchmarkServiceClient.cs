// using BenchmarkService;
// using CleanModels.Benchmark;
// using CleanModels.Commands.Base;
// using CleanModels.Queries.Base;
// using Client.Clients.Base;
// using static BenchmarkService.Benchmark;
// using PageModel = CleanModels.PageModel;
//
// namespace Client.Clients;
//
// /// <inheritdoc />
// public class TrustedHardwareBenchmarkServiceClient
//     : EntityServiceClientBase<BenchmarkClient, TrustedHardwareBenchmark>
// {
//     /// <summary>
//     /// Initializes a new instance of the <see cref="TrustedHardwareBenchmarkServiceClient"/> class.
//     /// </summary>
//     /// <param name="client">Client.</param>
//     public TrustedHardwareBenchmarkServiceClient(BenchmarkClient client)
//          : base(client)
//     {
//     }
//
//     /// <inheritdoc/>
//     public override async Task<QueryResult<List<TrustedHardwareBenchmark>>> GetAsync(PageModel pageModel)
//     {
//         var request = new GetRequest { PageModel = pageModel.MapToProtoPageModel() };
//         var response = await Client.GetTrustedHardwareBenchmarkAsync(request);
//         if (!string.IsNullOrEmpty(response.ErrorMessage))
//         {
//             return new QueryResult<List<TrustedHardwareBenchmark>>(response.ErrorMessage);
//         }
//
//         var benchmarks = response.Data.Benchmarks
//             .Select(protoBenchmark => protoBenchmark.MapToTrustedHardwareBenchmark())
//             .ToList();
//
//         return new QueryResult<List<TrustedHardwareBenchmark>>(benchmarks);
//     }
//
//     /// <inheritdoc/>
//     public override async Task<CommandResult> AddAsync(TrustedHardwareBenchmark model)
//     {
//         var request = new AddOrUpdateTrustedHardwareBenchmarkRequest()
//             { Benchmark = model.MapToProtoTrustedHardwareBenchmark() };
//
//         var response = await Client.AddTrustedHardwareBenchmarkAsync(request);
//         if (!string.IsNullOrEmpty(response.ErrorMessage))
//         {
//             return new CommandResult(response.ErrorMessage);
//         }
//
//         return new CommandResult();
//     }
//
//     /// <inheritdoc/>
//     public override async Task<CommandResult> UpdateAsync(TrustedHardwareBenchmark model)
//     {
//         var request = new AddOrUpdateTrustedHardwareBenchmarkRequest
//             { Benchmark = model.MapToProtoTrustedHardwareBenchmark() };
//
//         var response = await Client.UpdateTrustedHardwareBenchmarkAsync(request);
//         if (!string.IsNullOrEmpty(response.ErrorMessage))
//         {
//             return new CommandResult(response.ErrorMessage);
//         }
//
//         return new CommandResult();
//     }
//
//     /// <inheritdoc/>
//     public override async Task<CommandResult> RemoveAsync(List<TrustedHardwareBenchmark> model)
//     {
//         var request = new RemoveTrustedHardwareBenchmarksRequest();
//         request.Benchmarks.AddRange(model.Select(benchmark => benchmark.MapToProtoTrustedHardwareBenchmark()));
//         var response = await Client.RemoveTrustedHardwareBenchmarksAsync(request);
//         if (!string.IsNullOrEmpty(response.ErrorMessage))
//         {
//             return new CommandResult(response.ErrorMessage);
//         }
//
//         return new CommandResult();
//     }
// }
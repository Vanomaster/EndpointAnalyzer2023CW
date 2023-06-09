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
// public class TrustedSoftwareBenchmarkServiceClient
//     : EntityServiceClientBase<BenchmarkClient, TrustedSoftwareBenchmark>
// {
//     /// <summary>
//     /// Initializes a new instance of the <see cref="TrustedSoftwareBenchmarkServiceClient"/> class.
//     /// </summary>
//     /// <param name="client">Client.</param>
//     public TrustedSoftwareBenchmarkServiceClient(BenchmarkClient client)
//          : base(client)
//     {
//     }
//
//     /// <inheritdoc/>
//     public override async Task<QueryResult<List<TrustedSoftwareBenchmark>>> GetAsync(PageModel pageModel)
//     {
//         var request = new GetRequest { PageModel = pageModel.MapToProtoPageModel() };
//         var response = await Client.GetTrustedSoftwareBenchmarkAsync(request);
//         if (!string.IsNullOrEmpty(response.ErrorMessage))
//         {
//             return new QueryResult<List<TrustedSoftwareBenchmark>>(response.ErrorMessage);
//         }
//
//         var benchmarks = response.Data.Benchmarks
//             .Select(protoBenchmark => protoBenchmark.MapToTrustedSoftwareBenchmark())
//             .ToList();
//
//         return new QueryResult<List<TrustedSoftwareBenchmark>>(benchmarks);
//     }
//
//     /// <inheritdoc/>
//     public override async Task<CommandResult> AddAsync(TrustedSoftwareBenchmark model)
//     {
//         var request = new AddOrUpdateTrustedSoftwareBenchmarkRequest
//             { Benchmark = model.MapToProtoTrustedSoftwareBenchmark() };
//
//         var response = await Client.AddTrustedSoftwareBenchmarkAsync(request);
//         if (!string.IsNullOrEmpty(response.ErrorMessage))
//         {
//             return new CommandResult(response.ErrorMessage);
//         }
//
//         return new CommandResult();
//     }
//
//     /// <inheritdoc/>
//     public override async Task<CommandResult> UpdateAsync(TrustedSoftwareBenchmark model)
//     {
//         var request = new AddOrUpdateTrustedSoftwareBenchmarkRequest
//             { Benchmark = model.MapToProtoTrustedSoftwareBenchmark() };
//
//         var response = await Client.UpdateTrustedSoftwareBenchmarkAsync(request);
//         if (!string.IsNullOrEmpty(response.ErrorMessage))
//         {
//             return new CommandResult(response.ErrorMessage);
//         }
//
//         return new CommandResult();
//     }
//
//     /// <inheritdoc/>
//     public override async Task<CommandResult> RemoveAsync(List<TrustedSoftwareBenchmark> model)
//     {
//         var request = new RemoveTrustedSoftwareBenchmarksRequest();
//         request.Benchmarks.AddRange(model.Select(benchmark => benchmark.MapToProtoTrustedSoftwareBenchmark()));
//         var response = await Client.RemoveTrustedSoftwareBenchmarksAsync(request);
//         if (!string.IsNullOrEmpty(response.ErrorMessage))
//         {
//             return new CommandResult(response.ErrorMessage);
//         }
//
//         return new CommandResult();
//     }
// }
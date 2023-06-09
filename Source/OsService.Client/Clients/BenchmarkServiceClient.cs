// using CleanModels.Commands.Base;
// using CleanModels.Queries.Base;
// using OsService.Client.Clients.Base;
// using Benchmark = CleanModels.Benchmark;
// using PageModel = CleanModels.PageModel;
//
// namespace OsService.Client.Clients;
//
// /// <inheritdoc />
// public class BenchmarkServiceClient : EntityServiceClientBase<BenchmarkClient, Benchmark>
// {
//     /// <summary>
//     /// Initializes a new instance of the <see cref="BenchmarkServiceClient"/> class.
//     /// </summary>
//     /// <param name="client">Client.</param>
//     public BenchmarkServiceClient(BenchmarkClient client)
//          : base(client)
//     {
//     }
//
//     public override async Task<QueryResult<List<Benchmark>>> GetAsync(PageModel pageModel)
//     {
//         var request = new GetRequest { PageModel = pageModel.MapToProtoPageModel() };
//         var response = await Client.GetAsync(request);
//         var benchmarks = response.Data.Benchmarks.Select(protoBenchmark => protoBenchmark.MapToBenchmark()).ToList();
//         // switch (response.QueryResultCase)
//         // {
//         //     case GetResponse.QueryResultOneofCase.None:
//         //         break;
//         //     case GetResponse.QueryResultOneofCase.Data:
//         //         return new QueryResult<List<Benchmark>>(benchmarks);
//         //     case GetResponse.QueryResultOneofCase.CommandResponse:
//         //         return new QueryResult<List<Benchmark>>(response.CommandResponse.ErrorMessage);
//         //     default:
//         //         throw new ArgumentOutOfRangeException();
//         // }
//
//         if (!string.IsNullOrEmpty(response.ErrorMessage))
//         {
//             return new QueryResult<List<Benchmark>>(response.ErrorMessage);
//         }
//
//         return new QueryResult<List<Benchmark>>(benchmarks);
//     }
//
//     public override async Task<CommandResult> AddAsync(Benchmark model)
//     {
//         throw new NotImplementedException();
//     }
//
//     public override async Task<CommandResult> AddOrUpdateAsync(Benchmark model)
//     {
//         var request = new AddOrUpdateRequest { Benchmark = model.MapToProtoBenchmark() };
//         var response = await Client.AddOrUpdateAsync(request);
//         if (!string.IsNullOrEmpty(response.ErrorMessage))
//         {
//             return new CommandResult(response.ErrorMessage); // TODO convert messages to locale readable format
//         }
//
//         // TODO ex handling
//
//         return new CommandResult();
//     }
//
//     // public override async Task<CommandResult> AddOrUpdateFromFileAsync(byte[] bytes)
//     // {
//     //     var q = new Benchmark().MapToProtoBenchmark();
//     //     var byteString = UnsafeByteOperations.UnsafeWrap(bytes);
//     //     var request = new AddOrUpdateFromFileRequest { Bytes = byteString };
//     //     var response = await Client.AddOrUpdateFromFileAsync(request);
//     //     if (string.IsNullOrEmpty(response.ErrorMessage))
//     //     {
//     //         return new CommandResult(response.ErrorMessage);
//     //     }
//     //
//     //     return new CommandResult();
//     // }
// }
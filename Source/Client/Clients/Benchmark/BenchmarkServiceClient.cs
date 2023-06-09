using BenchmarkService;
using CleanModels.Commands.Base;
using CleanModels.Queries.Base;
using Client.Clients.Base;
using static BenchmarkService.Benchmark;
using Benchmark = CleanModels.Benchmark.Benchmark;
using PageModel = CleanModels.PageModel;

namespace Client.Clients;

/// <inheritdoc />
public class BenchmarkServiceClient : EntityServiceClientBase<BenchmarkClient, Benchmark>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BenchmarkServiceClient"/> class.
    /// </summary>
    /// <param name="client">Client.</param>
    public BenchmarkServiceClient(BenchmarkClient client)
         : base(client)
    {
    }

    /// <inheritdoc/>
    public override async Task<QueryResult<List<Benchmark>>> GetAsync(PageModel pageModel)
    {
        var request = new GetRequest { PageModel = pageModel.MapToProtoPageModel() };
        var response = await Client.GetAsync(request);
        // switch (response.QueryResultCase)
        // {
        //     case GetResponse.QueryResultOneofCase.None:
        //         break;
        //     case GetResponse.QueryResultOneofCase.Data:
        //         return new QueryResult<List<Benchmark>>(benchmarks);
        //     case GetResponse.QueryResultOneofCase.CommandResponse:
        //         return new QueryResult<List<Benchmark>>(response.CommandResponse.ErrorMessage);
        //     default:
        //         throw new ArgumentOutOfRangeException();
        // }

        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new QueryResult<List<Benchmark>>(response.ErrorMessage); // TODO convert messages to locale readable format
        }

        // TODO ex handling
        var benchmarks = response.Data.Benchmarks.Select(protoBenchmark => protoBenchmark.MapToBenchmark()).ToList();

        return new QueryResult<List<Benchmark>>(benchmarks);
    }

    /// <inheritdoc/>
    public override async Task<QueryResult<List<string>>> GetAllNamesAsync()
    {
        var request = new GetAllNamesRequest();
        var response = await Client.GetAllNamesAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new QueryResult<List<string>>(response.ErrorMessage);
        }

        return new QueryResult<List<string>>(response.Data.Names.ToList());
    }

    /// <inheritdoc/>
    public override async Task<CommandResult> AddAsync(Benchmark model)
    {
        var request = new AddOrUpdateRequest { Benchmark = model.MapToProtoBenchmark() };
        var response = await Client.AddAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new CommandResult(response.ErrorMessage);
        }

        return new CommandResult();
    }

    /// <inheritdoc/>
    public override async Task<CommandResult> UpdateAsync(Benchmark model)
    {
        var request = new AddOrUpdateRequest { Benchmark = model.MapToProtoBenchmark() };
        var response = await Client.UpdateAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new CommandResult(response.ErrorMessage);
        }

        return new CommandResult();
    }

    /// <inheritdoc/>
    public override async Task<CommandResult> RemoveAsync(IEnumerable<Benchmark> model)
    {
        var request = new RemoveRequest();
        request.Benchmarks.AddRange(model.Select(benchmark => benchmark.MapToProtoBenchmark()));
        var response = await Client.RemoveAsync(request);
        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            return new CommandResult(response.ErrorMessage);
        }

        return new CommandResult();
    }

    // public override async Task<CommandResult> AddOrUpdateAsync(Benchmark model)
    // {
    //     var request = new AddOrUpdateMainBenchmarkRequest { Benchmark = model.MapToProtoBenchmark() };
    //     var response = await Client.AddOrUpdateAsync(request);
    //     if (!string.IsNullOrEmpty(response.ErrorMessage))
    //     {
    //         return new CommandResult(response.ErrorMessage);
    //     }
    //
    //     return new CommandResult();
    // }

    // public override async Task<CommandResult> AddOrUpdateFromFileAsync(byte[] bytes)
    // {
    //     var q = new Benchmark().MapToProtoBenchmark();
    //     var byteString = UnsafeByteOperations.UnsafeWrap(bytes);
    //     var request = new AddOrUpdateFromFileRequest { Bytes = byteString };
    //     var response = await Client.AddOrUpdateFromFileAsync(request);
    //     if (string.IsNullOrEmpty(response.ErrorMessage))
    //     {
    //         return new CommandResult(response.ErrorMessage);
    //     }
    //
    //     return new CommandResult();
    // }
}
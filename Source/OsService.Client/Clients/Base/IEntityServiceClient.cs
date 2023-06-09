using CleanModels;
using CleanModels.Commands.Base;
using CleanModels.Queries.Base;
using Grpc.Core;

namespace OsService.Client.Clients.Base;

/// <summary>
/// .
/// </summary>
/// <typeparam name="TClient">..</typeparam>
/// <typeparam name="TModel">...</typeparam>
public interface IEntityServiceClient<in TClient, TModel>
    where TClient : ClientBase
{
    public Task<QueryResult<List<TModel>>> GetAsync(PageModel pageModel);

    public Task<CommandResult> AddAsync(TModel model);

    public Task<CommandResult> AddOrUpdateAsync(TModel model);

    // public Task<CommandResult> AddOrUpdateFromFileAsync(byte[] bytes);
}
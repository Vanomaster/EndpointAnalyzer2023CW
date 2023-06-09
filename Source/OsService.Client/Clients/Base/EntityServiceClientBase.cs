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
public abstract class EntityServiceClientBase<TClient, TModel> :
    ServiceClientBase<TClient>, IEntityServiceClient<TClient, TModel>
    where TClient : ClientBase
{
    protected EntityServiceClientBase(TClient client)
        : base(client)
    {
    }

    public abstract Task<QueryResult<List<TModel>>> GetAsync(PageModel pageModel);

    public abstract Task<CommandResult> AddAsync(TModel model);

    public abstract Task<CommandResult> AddOrUpdateAsync(TModel model);

    // public abstract Task<CommandResult> AddOrUpdateFromFileAsync(byte[] bytes);
}
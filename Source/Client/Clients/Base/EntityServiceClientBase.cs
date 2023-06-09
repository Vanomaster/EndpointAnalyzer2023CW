using CleanModels;
using CleanModels.Commands.Base;
using CleanModels.Queries.Base;
using Grpc.Core;

namespace Client.Clients.Base;

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

    /// <inheritdoc/>
    public abstract Task<QueryResult<List<TModel>>> GetAsync(PageModel pageModel);

    public abstract Task<QueryResult<List<string>>> GetAllNamesAsync();

    /// <inheritdoc/>
    public abstract Task<CommandResult> AddAsync(TModel model);

    /// <inheritdoc/>
    public abstract Task<CommandResult> UpdateAsync(TModel model);

    // public abstract Task<CommandResult> AddOrUpdateAsync(TModel model);

    /// <inheritdoc/>
    public abstract Task<CommandResult> RemoveAsync(IEnumerable<TModel> model);
}
using CleanModels.Queries.Base;
using Grpc.Core;

namespace Client.Clients.Base;

/// <summary>
/// .
/// </summary>
/// <typeparam name="TClient">..</typeparam>
/// <typeparam name="TModel">...</typeparam>
/// <typeparam name="TResult">....</typeparam>
public abstract class ReadOnlyServiceClientBase<TClient, TResult> :
    ServiceClientBase<TClient>, IReadOnlyServiceClient<TClient, TResult>
    where TClient : ClientBase
{
    protected ReadOnlyServiceClientBase(TClient client)
        : base(client)
    {
    }

    /// <inheritdoc/>
    public abstract Task<QueryResult<List<TResult>>> GetAsync();
}
using CleanModels.Queries.Base;
using Grpc.Core;

namespace Client.Clients.Base;

/// <summary>
/// .
/// </summary>
/// <typeparam name="TClient">..</typeparam>
/// <typeparam name="TModel">...</typeparam>
/// <typeparam name="TResult">....</typeparam>
public interface IReadOnlyServiceClient<in TClient, in TModel, TResult>
    where TClient : ClientBase
{
    public Task<QueryResult<List<TResult>>> GetAsync(TModel model);
}
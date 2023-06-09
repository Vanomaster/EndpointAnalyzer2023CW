using CleanModels.Queries.Base;

namespace Server.Client.Clients.Base;

/// <summary>
/// .
/// </summary>
/// <typeparam name="TResult">..</typeparam>
/// <typeparam name="TModel">...</typeparam>
public interface IServiceClient<TModel, TResult>
{
    public Task<QueryResult<TResult>> GetAsync(string pcIp, TModel model);
}
using CleanModels.Queries.Base;

namespace Server.Client.Clients.Base;

/// <summary>
/// .
/// </summary>
/// <typeparam name="TResult">..</typeparam>
public interface IServiceClient<TResult>
{
    public Task<QueryResult<TResult>> GetAsync(string pcIp);
}
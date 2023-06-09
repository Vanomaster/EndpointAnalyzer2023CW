using CleanModels.Queries.Base;

namespace Server.Client.Clients.Base;

/// <summary>
/// .
/// </summary>
/// <typeparam name="TResult">..</typeparam>
public interface IUpgradableSoftwareInfoServiceClient
{
    public Task<QueryResult<string>> GetAsync(string pcIp);
}
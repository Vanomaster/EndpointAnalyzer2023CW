using CleanModels.Queries.Base;

namespace Server.Client.Clients.Base;

/// <summary>
/// .
/// </summary>
public interface IInstalledSoftwareInfoServiceClient
{
    public Task<QueryResult<string>> GetAsync(string pcIp);
}
using CleanModels.Queries.Base;

namespace Server.Client.Clients.Base;

/// <summary>
/// .
/// </summary>
public interface IUsbHardwareInfoServiceClient
{
    public Task<QueryResult<string>> GetAsync(string pcIp);
}
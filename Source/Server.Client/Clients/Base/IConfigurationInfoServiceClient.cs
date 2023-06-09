using CleanModels;
using CleanModels.Queries.Base;

namespace Server.Client.Clients.Base;

/// <summary>
/// .
/// </summary>
public interface IConfigurationInfoServiceClient
{
    public Task<QueryResult<List<ConfigurationVerification>>> GetAsync(string pcIp, List<string> verificationCommands);
}
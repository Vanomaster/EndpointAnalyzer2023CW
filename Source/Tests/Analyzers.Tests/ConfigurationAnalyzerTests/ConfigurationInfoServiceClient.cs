using CleanModels;
using CleanModels.Queries.Base;
using Server.Client.Clients.Base;

namespace Analyzers.Tests;

public class ConfigurationInfoServiceClient : IConfigurationInfoServiceClient
{
    public async Task<QueryResult<List<ConfigurationVerification>>> GetAsync(
        string configurationVerificationPaths,
        List<string> empty)
    {
        string[] paths = configurationVerificationPaths.Split(" #|# ");
        string configurationVerificationCommandFilePath = Path.GetFullPath(paths[0]);
        string configurationVerificationResultFilePath = Path.GetFullPath(paths[1]);
        var configurationVerifications = new List<ConfigurationVerification>
        {
            new ()
            {
                VerificationCommand = await File.ReadAllTextAsync(configurationVerificationCommandFilePath),
                VerificationResult = await File.ReadAllTextAsync(configurationVerificationResultFilePath),
            },
        };

        return new QueryResult<List<ConfigurationVerification>>(data: configurationVerifications);
    }
}
using CleanModels.Queries.Base;
using Server.Client.Clients.Base;

namespace Analyzers.Tests;

public class UpgradableSoftwareInfoServiceClient : IUpgradableSoftwareInfoServiceClient
{
    public async Task<QueryResult<string>> GetAsync(string modelPath)
    {
        if (modelPath == "Fail_Test")
        {
            return new QueryResult<string>("Fail");
        }

        string modelFilePath = Path.GetFullPath(modelPath);
        string model = await File.ReadAllTextAsync(modelFilePath);

        return new QueryResult<string>(data: model);
    }
}
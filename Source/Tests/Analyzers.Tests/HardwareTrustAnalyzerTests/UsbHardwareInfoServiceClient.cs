using CleanModels.Queries.Base;
using Server.Client.Clients.Base;

namespace Analyzers.Tests;

public class UsbHardwareInfoServiceClient : IUsbHardwareInfoServiceClient
{
    public async Task<QueryResult<string>> GetAsync(string modelPath)
    {
        string modelFilePath = Path.GetFullPath(modelPath);
        string model = await File.ReadAllTextAsync(modelFilePath);

        return new QueryResult<string>(data: model);
    }
}
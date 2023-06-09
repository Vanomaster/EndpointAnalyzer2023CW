using CleanModels.Queries.Base;
using Dal.Entities;
using Newtonsoft.Json;

namespace Analyzers.Tests;

public class TrustedHardwareByBenchmarkNameQuery : IQuery<string, List<TrustedHardware>>
{
    public async Task<QueryResult<List<TrustedHardware>>> ExecuteAsync(string modelPath)
    {
        if (modelPath == "Fail_Test")
        {
            return new QueryResult<List<TrustedHardware>>("Fail");
        }

        string modelFilePath = Path.GetFullPath(modelPath);
        string model = await File.ReadAllTextAsync(modelFilePath);
        var trustedHardware = JsonConvert.DeserializeObject<List<TrustedHardware>>(model)
                              ?? new List<TrustedHardware>();

        return new QueryResult<List<TrustedHardware>>(data: trustedHardware);
    }
}
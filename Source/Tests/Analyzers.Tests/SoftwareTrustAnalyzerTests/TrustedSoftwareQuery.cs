using CleanModels.Queries.Base;
using Dal.Entities;
using Newtonsoft.Json;

namespace Analyzers.Tests;

public class TrustedSoftwareByBenchmarkNameQuery : IQuery<string, List<TrustedSoftware>>
{
    public async Task<QueryResult<List<TrustedSoftware>>> ExecuteAsync(string modelPath)
    {
        if (modelPath == "Fail_Test")
        {
            return new QueryResult<List<TrustedSoftware>>("Fail");
        }

        string modelFilePath = Path.GetFullPath(modelPath);
        string model = await File.ReadAllTextAsync(modelFilePath);
        var trustedSoftware = JsonConvert.DeserializeObject<List<TrustedSoftware>>(model) ?? new List<TrustedSoftware>();

        return new QueryResult<List<TrustedSoftware>>(data: trustedSoftware);
    }
}
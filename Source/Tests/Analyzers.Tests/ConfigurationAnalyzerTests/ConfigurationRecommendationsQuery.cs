using CleanModels.Queries.Base;
using Dal.Entities;
using Newtonsoft.Json;

namespace Analyzers.Tests;

public class ConfigurationRecommendationsByBenchmarkNameQuery : IQuery<string, List<ConfigurationRecommendation>>
{
    public async Task<QueryResult<List<ConfigurationRecommendation>>> ExecuteAsync(string modelPath)
    {
        if (modelPath == "Fail_Test")
        {
            return new QueryResult<List<ConfigurationRecommendation>>("Fail");
        }

        string modelFilePath = Path.GetFullPath(modelPath);
        string model = await File.ReadAllTextAsync(modelFilePath);
        var configurationRecommendation = JsonConvert.DeserializeObject<List<ConfigurationRecommendation>>(model)
                                          ?? new List<ConfigurationRecommendation>();

        return new QueryResult<List<ConfigurationRecommendation>>(data: configurationRecommendation);
    }
}
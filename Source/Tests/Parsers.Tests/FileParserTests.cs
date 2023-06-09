using CleanModels.Benchmark;
using Client.Services.Mappers.Csv;
using Client.Services.Parsers;
using Newtonsoft.Json;
using Xunit;

namespace Parsers.Tests;

public class FileParserTests
{
    [Theory]
    [InlineData("TestData/FileParser/Text.txt", "TestData/FileParser/Model.txt")]
    public async Task Parse_Text_ReturnsModels(string textPath, string modelPath)
    {
        string modelFilePath = Path.GetFullPath(modelPath);
        string expected = await File.ReadAllTextAsync(modelFilePath);
        string textFilePath = Path.GetFullPath(textPath);
        var parser = new FileParser<ConfigurationRecommendation, ConfigurationRecommendationMapper>();

        var queryResult = parser.Parse(textFilePath);
        if (!queryResult.IsSuccessful)
        {
            Assert.Fail($"Test failed. {queryResult.ErrorMessage}");
        }

        var configurationRecommendations = queryResult.Data;
        string actual = JsonConvert.SerializeObject(configurationRecommendations.FirstOrDefault());

        Assert.Equal(expected, actual);
    }
}
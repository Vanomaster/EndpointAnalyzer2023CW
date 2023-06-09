using CleanModels.Analysis;
using Parsers;
using Xunit;

namespace Analyzers.Tests;

public class SoftwareUpdateAnalyzerTests // TODO create error tests
{
    [Theory]
    [InlineData(
        "TestData/SoftwareUpdateAnalyzer/SoftwareIsUpToDate/UpgradableSoftware.txt",
        "TestData/SoftwareUpdateAnalyzer/SoftwareIsUpToDate/Recommendations.txt")]
    [InlineData(
        "TestData/SoftwareUpdateAnalyzer/UpgradableSoftware/UpgradableSoftware.txt",
        "TestData/SoftwareUpdateAnalyzer/UpgradableSoftware/Recommendations.txt")]
    public async Task AnalyzeAsync_AnalysisModel_Recommendations(string modelPath, string recommendationsPath)
    {
        string recommendationsFilePath = Path.GetFullPath(recommendationsPath);
        //string expected = await File.ReadAllTextAsync(recommendationsFilePath);
        byte[] expected = await File.ReadAllBytesAsync(recommendationsFilePath);
        var analyzer = new SoftwareUpdateAnalyzer(
            new UpgradableSoftwareInfoServiceClient(),
            new UpgradableSoftwareParser());

        var analysisModel = new AnalysisModel
        {
            PcIp = modelPath,
        };

        var queryResult = await analyzer.AnalyzeAsync(analysisModel);
        if (!queryResult.IsSuccessful)
        {
            Assert.Fail($"Test failed. {queryResult.ErrorMessage}");
        }

        byte[] actual = queryResult.Data;
        // var actual1 = actual.ToObject<List<ConfigurationRecommendation>>();
        // await File.WriteAllBytesAsync(recommendationsFilePath, actual);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task AnalyzeAsync_FailAnalysisModel_Fail()
    {
        var analyzer = new SoftwareUpdateAnalyzer(
            new UpgradableSoftwareInfoServiceClient(),
            new UpgradableSoftwareParser());

        var analysisModel = new AnalysisModel
        {
            PcIp = "Fail_Test",
        };

        var queryResult = await analyzer.AnalyzeAsync(analysisModel);
        Assert.True(!queryResult.IsSuccessful);
    }
}
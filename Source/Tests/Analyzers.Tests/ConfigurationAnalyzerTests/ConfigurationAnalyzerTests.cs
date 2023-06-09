using CleanModels.Analysis;
using CleanModels.Benchmark;
using Common.Extensions;
using Xunit;

namespace Analyzers.Tests;

public class ConfigurationAnalyzerTests
{
    [Theory]
    [InlineData(
        "TestData/ConfigurationAnalyzer/VerificationResultMatches/Configuration.txt",
        "TestData/ConfigurationAnalyzer/VerificationResultMatches/Recommendations.txt")]
    [InlineData(
        "TestData/ConfigurationAnalyzer/VerificationResultNotMatches/Configuration.txt",
        "TestData/ConfigurationAnalyzer/VerificationResultNotMatches/Recommendations.txt")]
    public async Task AnalyzeAsync_AnalysisModel_Recommendations(
        string configurationPath,
        string recommendationsPath)
    {
        const string configurationRecommendationsPath = "TestData/ConfigurationAnalyzer/ConfigurationRecommendations.txt";
        const string configurationVerificationCommandPath = "TestData/ConfigurationAnalyzer/ConfigurationVerificationCommand.txt";
        string recommendationsFilePath = Path.GetFullPath(recommendationsPath);
        byte[] expected = await File.ReadAllBytesAsync(recommendationsFilePath);
        var analyzer = new ConfigurationAnalyzer(
            new ConfigurationInfoServiceClient(),
            new ConfigurationRecommendationsByBenchmarkNameQuery());

        var analysisModel = new AnalysisModel
        {
            PcIp = configurationVerificationCommandPath + " #|# " + configurationPath,
            BenchmarkName = configurationRecommendationsPath,
        };

        var queryResult = await analyzer.AnalyzeAsync(analysisModel);
        if (!queryResult.IsSuccessful)
        {
            Assert.Fail($"Test failed. {queryResult.ErrorMessage}");
        }

        byte[] actual = queryResult.Data;

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("TestData/ConfigurationAnalyzer/Fail/Configuration.txt")]
    public async Task AnalyzeAsync_FailAnalysisModel_Fail(string configurationPath)
    {
        const string configurationVerificationCommandPath = "TestData/ConfigurationAnalyzer/ConfigurationVerificationCommand.txt";
        var analyzer = new ConfigurationAnalyzer(
            new ConfigurationInfoServiceClient(),
            new ConfigurationRecommendationsByBenchmarkNameQuery());

        var analysisModel = new AnalysisModel
        {
            PcIp = configurationVerificationCommandPath + " #|# " + configurationPath,
            BenchmarkName = "Fail_Test",
        };

        var queryResult = await analyzer.AnalyzeAsync(analysisModel);
        Assert.True(!queryResult.IsSuccessful);
    }
}
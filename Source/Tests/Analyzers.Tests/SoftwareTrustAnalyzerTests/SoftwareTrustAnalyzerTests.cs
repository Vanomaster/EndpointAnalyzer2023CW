using CleanModels.Analysis;
using Parsers;
using Xunit;

namespace Analyzers.Tests;

public class SoftwareTrustAnalyzerTests
{
    [Theory]
    [InlineData(
        "TestData/SoftwareTrustAnalyzer/InstalledSoftware.txt",
        "TestData/SoftwareTrustAnalyzer/VersionsEquals/TrustedSoftware.txt",
        "TestData/SoftwareTrustAnalyzer/VersionsEquals/Recommendations.txt")]
    [InlineData(
        "TestData/SoftwareTrustAnalyzer/InstalledSoftware.txt",
        "TestData/SoftwareTrustAnalyzer/TrustedSoftwareVersionFollows/TrustedSoftware.txt",
        "TestData/SoftwareTrustAnalyzer/TrustedSoftwareVersionFollows/Recommendations.txt")]
    [InlineData(
        "TestData/SoftwareTrustAnalyzer/InstalledSoftware.txt",
        "TestData/SoftwareTrustAnalyzer/TrustedSoftwareVersionPrecedes/TrustedSoftware.txt",
        "TestData/SoftwareTrustAnalyzer/TrustedSoftwareVersionPrecedes/Recommendations.txt")]
    public async Task AnalyzeAsync_AnalysisModel_Recommendations(
        string installedSoftwarePath,
        string trustedSoftwarePath,
        string recommendationsPath)
    {
        string recommendationsFilePath = Path.GetFullPath(recommendationsPath);
        byte[] expected = await File.ReadAllBytesAsync(recommendationsFilePath);
        var analyzer = new SoftwareTrustAnalyzer(
            new InstalledSoftwareInfoServiceClient(),
            new InstalledSoftwareParser(),
            new TrustedSoftwareByBenchmarkNameQuery());

        var analysisModel = new AnalysisModel
        {
            PcIp = installedSoftwarePath,
            BenchmarkName = trustedSoftwarePath,
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
    [InlineData("TestData/SoftwareTrustAnalyzer/InstalledSoftware.txt")]
    public async Task AnalyzeAsync_FailAnalysisModel_Fail(string installedSoftwarePath)
    {
        var analyzer = new SoftwareTrustAnalyzer(
            new InstalledSoftwareInfoServiceClient(),
            new InstalledSoftwareParser(),
            new TrustedSoftwareByBenchmarkNameQuery());

        var analysisModel = new AnalysisModel
        {
            PcIp = installedSoftwarePath,
            BenchmarkName = "Fail_Test",
        };

        var queryResult = await analyzer.AnalyzeAsync(analysisModel);
        Assert.True(!queryResult.IsSuccessful);
    }
}
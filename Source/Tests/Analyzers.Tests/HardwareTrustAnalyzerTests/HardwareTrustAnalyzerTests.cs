using CleanModels.Analysis;
using CleanModels.Hardware;
using Common.Extensions;
using Parsers;
using Xunit;

namespace Analyzers.Tests;

public class HardwareTrustAnalyzerTests
{
    [Theory]
    [InlineData(
        "TestData/HardwareTrustAnalyzer/InstalledHardware.txt",
        "TestData/HardwareTrustAnalyzer/TrustedHardware.txt",
        "TestData/HardwareTrustAnalyzer/Recommendations.txt")]
    public async Task AnalyzeAsync_AnalysisModel_Recommendations(
        string installedHardwarePath,
        string trustedHardwarePath,
        string recommendationsPath)
    {
        string recommendationsFilePath = Path.GetFullPath(recommendationsPath);
        byte[] expected = await File.ReadAllBytesAsync(recommendationsFilePath);
        var analyzer = new HardwareTrustAnalyzer(
            new UsbHardwareInfoServiceClient(),
            new InstalledHardwareParser(),
            new TrustedHardwareByBenchmarkNameQuery());

        var analysisModel = new AnalysisModel
        {
            PcIp = installedHardwarePath,
            BenchmarkName = trustedHardwarePath,
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
    [InlineData("TestData/HardwareTrustAnalyzer/InstalledHardware.txt")]
    public async Task AnalyzeAsync_FailAnalysisModel_Fail(
        string installedHardwarePath)
    {
        var analyzer = new HardwareTrustAnalyzer(
            new UsbHardwareInfoServiceClient(),
            new InstalledHardwareParser(),
            new TrustedHardwareByBenchmarkNameQuery());

        var analysisModel = new AnalysisModel
        {
            PcIp = installedHardwarePath,
            BenchmarkName = "Fail_Test",
        };

        var queryResult = await analyzer.AnalyzeAsync(analysisModel);
        Assert.True(!queryResult.IsSuccessful);
    }
}
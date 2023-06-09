using Analyzers.Base;
using CleanModels.Analysis;
using CleanModels.Hardware;
using CleanModels.Queries.Base;
using Common.Extensions;
using Dal.Entities;
using NLog;
using Parsers.Base;
using Server.Client.Clients.Base;

namespace Analyzers;

/// <summary>
/// Hardware trust analyzer.
/// </summary>
public class HardwareTrustAnalyzer : IAnalyzer<AnalysisModel, byte[]>
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    /// <summary>
    /// Initializes a new instance of the <see cref="HardwareTrustAnalyzer"/> class.
    /// </summary>
    /// <param name="usbHardwareInfoServiceClient">USB hardware info service client.</param>
    /// <param name="usbHardwareParser">USB hardware parser.</param>
    /// <param name="trustedHardwareQuery">Trusted hardware query.</param>
    public HardwareTrustAnalyzer(
        IUsbHardwareInfoServiceClient usbHardwareInfoServiceClient,
        IParser<string, List<UnknownHardware>> usbHardwareParser,
        IQuery<string, List<TrustedHardware>> trustedHardwareQuery)
    {
        UsbHardwareInfoServiceClient = usbHardwareInfoServiceClient;
        UsbHardwareParser = usbHardwareParser;
        TrustedHardwareByBenchmarkNameQuery = trustedHardwareQuery;
    }

    private IUsbHardwareInfoServiceClient UsbHardwareInfoServiceClient { get; }

    private IParser<string, List<UnknownHardware>> UsbHardwareParser { get; }

    private IQuery<string, List<TrustedHardware>> TrustedHardwareByBenchmarkNameQuery { get; }

    /// <inheritdoc/>
    public async Task<QueryResult<byte[]>> AnalyzeAsync(AnalysisModel analysisModel)
    {
        var usbHardwareQueryResult = await UsbHardwareInfoServiceClient.GetAsync(analysisModel.PcIp);
        if (!usbHardwareQueryResult.IsSuccessful)
        {
            return new QueryResult<byte[]>(usbHardwareQueryResult.ErrorMessage);
        }

        Logger.Info(usbHardwareQueryResult.Data);
        var usbHardwareParserResult = UsbHardwareParser.Parse(usbHardwareQueryResult.Data);
        if (!usbHardwareParserResult.IsSuccessful)
        {
            return new QueryResult<byte[]>(usbHardwareParserResult.ErrorMessage);
        }

        var trustedHardwareQueryResult =
            await TrustedHardwareByBenchmarkNameQuery.ExecuteAsync(analysisModel.BenchmarkName);

        if (!trustedHardwareQueryResult.IsSuccessful)
        {
            return new QueryResult<byte[]>(trustedHardwareQueryResult.ErrorMessage);
        }

        var untrustedHardware = GetUntrustedHardware(usbHardwareParserResult.Data, trustedHardwareQueryResult.Data);
        byte[] bytes = untrustedHardware.ToBytes();

        return new QueryResult<byte[]>(bytes);
    }

    private static List<UnknownHardware> GetUntrustedHardware(
        IEnumerable<UnknownHardware> hardware,
        IReadOnlyCollection<TrustedHardware> trustedHardware)
    {
        var untrustedHardware = hardware
            .DistinctBy(hardwareItem => hardwareItem.HardwareId)
            .Where(hardwareItem => trustedHardware
                .All(trustedHardwareItem => trustedHardwareItem.HardwareId != hardwareItem.HardwareId))
            .ToList();

        return untrustedHardware;
    }
}
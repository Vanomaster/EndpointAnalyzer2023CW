using Analyzers.Base;
using CleanModels.Analysis;
using CleanModels.Queries.Base;
using CleanModels.Software;
using Common.Extensions;
using NLog;
using Parsers.Base;
using Server.Client.Clients.Base;

namespace Analyzers;

/// <summary>
/// Software update analyzer.
/// </summary>
public class SoftwareUpdateAnalyzer : IAnalyzer<AnalysisModel, byte[]>
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    /// <summary>
    /// Initializes a new instance of the <see cref="SoftwareUpdateAnalyzer"/> class.
    /// </summary>
    /// <param name="upgradableSoftwareInfoServiceClient">Upgradable software info service client.</param>
    /// <param name="upgradableSoftwareParser">Upgradable software parser.</param>
    public SoftwareUpdateAnalyzer(
        IUpgradableSoftwareInfoServiceClient upgradableSoftwareInfoServiceClient,
        IParser<string, List<UpgradableSoftware>> upgradableSoftwareParser)
    {
        UpgradableSoftwareInfoServiceClient = upgradableSoftwareInfoServiceClient;
        UpgradableSoftwareParser = upgradableSoftwareParser;
    }

    private IUpgradableSoftwareInfoServiceClient UpgradableSoftwareInfoServiceClient { get; }

    private IParser<string, List<UpgradableSoftware>> UpgradableSoftwareParser { get; }

    /// <inheritdoc/>
    public async Task<QueryResult<byte[]>> AnalyzeAsync(AnalysisModel analysisModel)
    {
        var queryResult = await UpgradableSoftwareInfoServiceClient.GetAsync(analysisModel.PcIp);
        if (!queryResult.IsSuccessful)
        {
            return new QueryResult<byte[]>(queryResult.ErrorMessage);
        }

        Logger.Info(queryResult.Data);
        var parserResult = UpgradableSoftwareParser.Parse(queryResult.Data);
        if (!parserResult.IsSuccessful)
        {
            return new QueryResult<byte[]>(parserResult.ErrorMessage);
        }

        byte[] bytes = parserResult.Data.ToBytes();

        return new QueryResult<byte[]>(data: bytes);
    }

    // private static byte[] GetRecommendations(List<UpgradableSoftware> upgradableSoftware)
    // {
    //     return MemoryPackSerializer.Serialize(upgradableSoftware);
    // }

    // private static string GetRecommendations(List<UpgradableSoftware> upgradableSoftware)
    // {
    //     var recommendations = new StringBuilder();
    //     if (!upgradableSoftware.Any())
    //     {
    //         recommendations.Append(SoftwareUpdatesNotDetected);
    //
    //         return recommendations.ToString();
    //     }
    //
    //     recommendations.Append(@"Обнаружены обновления для следующих программ:" + "\n");
    //     recommendations.Append(MemoryPackSerializer.Serialize(upgradableSoftware));
    //
    //     return recommendations.ToString();
    // }
}
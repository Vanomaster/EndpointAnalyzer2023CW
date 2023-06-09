using Analyzers.Base;
using CleanModels.Analysis;
using CleanModels.Queries.Base;
using CleanModels.Software;
using Common.Extensions;
using Dal.Entities;
using Parsers.Base;
using Semver;
using Server.Client.Clients.Base;

namespace Analyzers;

/// <summary>
/// Software trust analyzer.
/// </summary>
public class SoftwareTrustAnalyzer : IAnalyzer<AnalysisModel, byte[]>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SoftwareTrustAnalyzer"/> class.
    /// </summary>
    /// <param name="installedSoftwareInfoServiceClient">Installed software info service client.</param>
    /// <param name="installedSoftwareParser">Installed software parser.</param>
    /// <param name="trustedSoftwareQuery">Trusted software query.</param>
    public SoftwareTrustAnalyzer(
        IInstalledSoftwareInfoServiceClient installedSoftwareInfoServiceClient,
        IParser<string, List<Software>> installedSoftwareParser,
        IQuery<string, List<TrustedSoftware>> trustedSoftwareQuery)
    {
        InstalledSoftwareInfoServiceClient = installedSoftwareInfoServiceClient;
        InstalledSoftwareParser = installedSoftwareParser;
        TrustedSoftwareByBenchmarkNameQuery = trustedSoftwareQuery;
    }

    private IInstalledSoftwareInfoServiceClient InstalledSoftwareInfoServiceClient { get; }

    private IParser<string, List<Software>> InstalledSoftwareParser { get; }

    private IQuery<string, List<TrustedSoftware>> TrustedSoftwareByBenchmarkNameQuery { get; }

    /// <inheritdoc/>
    public async Task<QueryResult<byte[]>> AnalyzeAsync(AnalysisModel analysisModel)
    {
        var installedSoftwareQueryResult = await InstalledSoftwareInfoServiceClient.GetAsync(analysisModel.PcIp);
        if (!installedSoftwareQueryResult.IsSuccessful)
        {
            return new QueryResult<byte[]>(installedSoftwareQueryResult.ErrorMessage);
        }

        var parserResult = InstalledSoftwareParser.Parse(installedSoftwareQueryResult.Data);
        if (!parserResult.IsSuccessful)
        {
            return new QueryResult<byte[]>(parserResult.ErrorMessage);
        }

        var trustedSoftwareQueryResult =
            await TrustedSoftwareByBenchmarkNameQuery.ExecuteAsync(analysisModel.BenchmarkName);

        if (!trustedSoftwareQueryResult.IsSuccessful)
        {
            return new QueryResult<byte[]>(trustedSoftwareQueryResult.ErrorMessage);
        }

        var untrustedSoftware = GetUntrustedSoftware(parserResult.Data, trustedSoftwareQueryResult.Data);
        byte[] bytes = untrustedSoftware.ToBytes();

        return new QueryResult<byte[]>(bytes);
    }

    private static List<Software> GetUntrustedSoftware(
        List<Software> software,
        IReadOnlyCollection<TrustedSoftware> trustedSoftware)
    {
        var untrustedSoftware = new List<Software>();
        foreach (var softwareItem in software)
        {
            var trustedSoftwareItems = trustedSoftware.Where(item => item.Name == softwareItem.Name).ToList();
            TrustedSoftware trustedSoftwareItem = default;
            if (trustedSoftwareItems.Count > 1)
            {
                var prevItem = trustedSoftwareItems[0];
                foreach (var item in trustedSoftwareItems)
                {
                    var prevItemVersion = SemVersion.Parse(prevItem.Version, SemVersionStyles.Any);
                    var itemVersion = SemVersion.Parse(item.Version, SemVersionStyles.Any);
                    bool prevIsPrecede = prevItemVersion.ComparePrecedenceTo(itemVersion) < 0;
                    if (prevIsPrecede)
                    {
                        prevItem = item;
                    }
                }

                trustedSoftwareItem = prevItem;
            }

            if (trustedSoftwareItems.Count == 1)
            {
                trustedSoftwareItem = trustedSoftwareItems.FirstOrDefault();
            }

            if (trustedSoftwareItem == default)
            {
                untrustedSoftware.Add(softwareItem);

                continue;
            }

            if (string.IsNullOrWhiteSpace(softwareItem.Version))
            {
                untrustedSoftware.Add(softwareItem);

                continue;
            }

            string softwareItemVersion = softwareItem.Version.Replace(':', '.');
            var softwareItemSemVersion = SemVersion.Parse(softwareItemVersion, SemVersionStyles.Any);
            var trustedSoftwareSemVersion = SemVersion.Parse(trustedSoftwareItem.Version, SemVersionStyles.Any);
            bool isPreviousVersion = softwareItemSemVersion.ComparePrecedenceTo(trustedSoftwareSemVersion) < 0;
            if (isPreviousVersion)
            {
                untrustedSoftware.Add(softwareItem);
            }
        }

        return untrustedSoftware;
    }

    // private static int CompareVersions(string first, string second)
    // {
    //     if (first == second)
    //     {
    //         return 0;
    //     }
    //
    //     if (first._Major != second._Major)
    //     {
    //         return first._Major <= second._Major ? -1 : 1;
    //     }
    //
    //     if (first._Minor != second._Minor)
    //     {
    //         return first._Minor <= second._Minor ? -1 : 1;
    //     }
    //
    //     if (first._Build != second._Build)
    //     {
    //         return first._Build <= second._Build ? -1 : 1;
    //     }
    //
    //     if (first._Revision == second._Revision)
    //     {
    //         return 0;
    //     }
    //
    //     return first._Revision <= second._Revision ? -1 : 1;
    //
    // }
}
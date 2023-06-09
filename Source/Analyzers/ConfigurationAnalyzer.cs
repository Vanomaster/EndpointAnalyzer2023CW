using Analyzers.Base;
using CleanModels;
using CleanModels.Analysis;
using CleanModels.Benchmark;
using CleanModels.Queries.Base;
using Common.Extensions;
using NLog;
using Server.Client.Clients.Base;
using ConfigurationRecommendation = Dal.Entities.ConfigurationRecommendation;
using ConfigurationRecommendationModel = CleanModels.Benchmark.ConfigurationRecommendation;

namespace Analyzers;

/// <summary>
/// Configuration analyzer.
/// </summary>
public class ConfigurationAnalyzer : IAnalyzer<AnalysisModel, byte[]>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationAnalyzer"/> class.
    /// </summary>
    /// <param name="configurationInfoServiceClient">Installed hardware info service client.</param>
    /// <param name="configurationRecommendationsQuery">Configuration recommendations query.</param>
    public ConfigurationAnalyzer(
        IConfigurationInfoServiceClient configurationInfoServiceClient,
        IQuery<string, List<ConfigurationRecommendation>> configurationRecommendationsQuery)
    {
        ConfigurationInfoServiceClient = configurationInfoServiceClient;
        ConfigurationRecommendationsByBenchmarkNameQuery = configurationRecommendationsQuery;
    }

    private IConfigurationInfoServiceClient ConfigurationInfoServiceClient { get; }

    private IQuery<string, List<ConfigurationRecommendation>> ConfigurationRecommendationsByBenchmarkNameQuery { get; }

    /// <inheritdoc/>
    public async Task<QueryResult<byte[]>> AnalyzeAsync(AnalysisModel analysisModel)
    {
        var configurationRecommendationsQueryResult =
            await ConfigurationRecommendationsByBenchmarkNameQuery.ExecuteAsync(analysisModel.BenchmarkName);

        if (!configurationRecommendationsQueryResult.IsSuccessful)
        {
            return new QueryResult<byte[]>(configurationRecommendationsQueryResult.ErrorMessage);
        }

        var verificationCommands = configurationRecommendationsQueryResult.Data
            .Select(rec => rec.VerificationCommand)
            .ToList();

        var configurationVerificationsQueryResult =
            await ConfigurationInfoServiceClient.GetAsync(analysisModel.PcIp, verificationCommands);

        if (!configurationVerificationsQueryResult.IsSuccessful)
        {
            return new QueryResult<byte[]>(configurationVerificationsQueryResult.ErrorMessage);
        }

        var unrecommendedConfigurations =
            GetUnrecommendedConfigurations(
                configurationVerificationsQueryResult.Data,
                configurationRecommendationsQueryResult.Data);

        byte[] bytes = unrecommendedConfigurations.ToBytes();

        return new QueryResult<byte[]>(bytes);
    }

    private static List<UnrecommendedConfiguration> GetUnrecommendedConfigurations(
        IReadOnlyCollection<ConfigurationVerification> configurationVerifications,
        List<ConfigurationRecommendation> configurationRecommendations)
    {
        var unrecommendedConfigurations = new List<UnrecommendedConfiguration>();
        foreach (var recommendation in configurationRecommendations)
        {
            string verificationResult = configurationVerifications
                .FirstOrDefault(v => v.VerificationCommand == recommendation.VerificationCommand)?.VerificationResult
                ?? string.Empty;

            if (verificationResult == recommendation.VerificationResult)
            {
                continue;
            }

            var unrecommendedConfiguration = new UnrecommendedConfiguration
            {
                Id = recommendation.Id,
                Name = recommendation.Name,
                VerificationCommand = recommendation.VerificationCommand,
                ExpectedVerificationResult = recommendation.VerificationResult,
                ActualVerificationResult = verificationResult,
                Configuration = new Configuration
                {
                    Id = recommendation.Configuration.Id,
                    Name = recommendation.Configuration.Name,
                    Description = recommendation.Configuration.Description,
                },
            };

            unrecommendedConfigurations.Add(unrecommendedConfiguration);
        }

        return unrecommendedConfigurations;
    }
}
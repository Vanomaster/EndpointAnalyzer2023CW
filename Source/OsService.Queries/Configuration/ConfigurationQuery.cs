using CleanModels;
using CleanModels.Queries.Base;
using NLog;

namespace OsService.Queries;

/// <inheritdoc />
public class ConfigurationQuery : CommandLineQueryBase<List<string>, List<ConfigurationVerification>>
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <inheritdoc/>
    protected override async Task<QueryResult<List<ConfigurationVerification>>> ExecuteCoreAsync(
        List<string> verificationCommands)
    {
        // var configurationVerifications = verificationCommands.Select(async c => new ConfigurationVerification
        // {
        //     VerificationCommand = c,
        //     VerificationResult = await ExecuteBashAsync(c),
        // }).ToList();
        var configurationVerifications = new List<ConfigurationVerification>(); // TODO parallel
        foreach (string command in verificationCommands)
        {
            string verificationResult = await ExecuteBashAsync(command);
            Logger.Info(verificationResult);
            configurationVerifications.Add(new ConfigurationVerification
            {
                VerificationCommand = command,
                VerificationResult = verificationResult,
            });
        }

        return GetSuccessfulResult(configurationVerifications);
    }
}
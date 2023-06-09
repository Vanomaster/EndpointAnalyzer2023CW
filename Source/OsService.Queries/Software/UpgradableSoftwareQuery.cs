using CleanModels.Queries.Base;
using NLog;

namespace OsService.Queries;

/// <inheritdoc />
public class UpgradableSoftwareQuery : CommandLineQueryBase<string>
{
    private const string AllSoftwareIsUpToDateText = @"Все пакеты имеют последние версии.";
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


    /// <inheritdoc/>
    protected override async Task<QueryResult<string>> ExecuteCoreAsync()
    {
        /*string connectedRepositories = await ExecuteBashAsync("sudo ");
        if (connectedRepositories.Contains())
        {
            return GetFailedResult();
        }*/

        string packetsCacheUpdateResult = await ExecuteBashAsync("sudo apt update");
        if (packetsCacheUpdateResult.Contains(AllSoftwareIsUpToDateText))
        {
            return GetSuccessfulResult(AllSoftwareIsUpToDateText);
        }

        string upgradableSoftware = await ExecuteBashAsync("sudo apt list --upgradable"); // aptitude search ~U
        Logger.Info(upgradableSoftware);

        return GetSuccessfulResult(upgradableSoftware);
    }
}
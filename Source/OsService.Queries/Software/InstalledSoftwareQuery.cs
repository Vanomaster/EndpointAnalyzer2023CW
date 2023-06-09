using CleanModels.Queries.Base;
using NLog;

namespace OsService.Queries;

/// <inheritdoc />
public class InstalledSoftwareQuery : CommandLineQueryBase<string>
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <inheritdoc/>
    protected override async Task<QueryResult<string>> ExecuteCoreAsync()
    {
        string installedSoftware = await ExecuteBashAsync("sudo dpkg -l"); // apt-mark showmanual | sort    |    aptitude search '~i!~M'   |   apt show <Name>
        Logger.Info(installedSoftware);

        return GetSuccessfulResult(installedSoftware);
    }
}
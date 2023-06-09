using CleanModels.Queries.Base;
using NLog;

namespace OsService.Queries;

/// <inheritdoc />
public class HostNameQuery : CommandLineQueryBase<string>
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <inheritdoc/>
    protected override async Task<QueryResult<string>> ExecuteCoreAsync()
    {
        string hostName = await ExecuteBashAsync("sudo hostname --fqdn"); // TODO sudo ?

        return GetSuccessfulResult(hostName);
    }
}
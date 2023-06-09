using CleanModels.Queries.Base;

namespace OsService.Queries;

/// <inheritdoc />
public class ConnectedPciHardwareQuery : CommandLineQueryBase<string>
{
    /// <inheritdoc/>
    protected override async Task<QueryResult<string>> ExecuteCoreAsync()
    {
        string connectedPciQuery = await ExecuteBashAsync("sudo lspci -nn");

        return GetSuccessfulResult(connectedPciQuery);
    }
}
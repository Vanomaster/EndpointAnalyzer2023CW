using CleanModels.Queries.Base;

namespace OsService.Queries;

/// <inheritdoc />
public class ServicesQuery : CommandLineQueryBase<string>
{
    /// <inheritdoc/>
    protected override async Task<QueryResult<string>> ExecuteCoreAsync()
    {
        string installedSoftware = await ExecuteBashAsync("sudo systemctl --type=service,target -a");

        return GetSuccessfulResult(installedSoftware);
    }
}
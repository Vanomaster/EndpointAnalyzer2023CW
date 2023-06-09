using CleanModels.Queries.Base;

namespace OsService.Queries;

/// <inheritdoc />
public class SoftwareInfoQuery : CommandLineQueryBase<List<string>, string>
{
    /// <inheritdoc/>
    protected override async Task<QueryResult<string>> ExecuteCoreAsync(List<string> softwareNames)
    {
        string command = softwareNames.Aggregate("sudo apt show ", (current, name) => current + (name + " "));
        string software = await ExecuteBashAsync(command);

        return GetSuccessfulResult(software);
    }
}
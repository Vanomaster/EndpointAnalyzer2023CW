using CleanModels.Commands.Base;
using Dal.Commands.Base;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Commands;

/// <inheritdoc />
public class AddNewTrustSoftCommand : DbCommandBase<List<TrustedSoftware>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddNewTrustSoftCommand"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public AddNewTrustSoftCommand(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult> ExecuteCoreAsync(List<TrustedSoftware> entitiesToAdd)
    {
        var parent = await Context.TrustedSoftwareBenchmarks.FirstOrDefaultAsync(b => b.Id == entitiesToAdd[0].TrustedSoftwareBenchmarks[0].Id);
        entitiesToAdd.ForEach(e => e.TrustedSoftwareBenchmarks.Clear());
        await Context.AddRangeAsync(entitiesToAdd);
        parent.TrustedSoftware.AddRange(entitiesToAdd);
        await Context.SaveChangesAsync();

        return GetSuccessfulResult();
    }
}
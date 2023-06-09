using CleanModels.Commands.Base;
using Dal.Commands.Base;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Commands;

/// <inheritdoc />
public class AddNewTrustHardCommand : DbCommandBase<List<TrustedHardware>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddNewTrustHardCommand"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public AddNewTrustHardCommand(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult> ExecuteCoreAsync(List<TrustedHardware> entitiesToAdd)
    {
        var parent = await Context.TrustedHardwareBenchmarks.FirstOrDefaultAsync(b => b.Id == entitiesToAdd[0].TrustedHardwareBenchmarks[0].Id);
        entitiesToAdd.ForEach(e => e.TrustedHardwareBenchmarks.Clear());
        await Context.AddRangeAsync(entitiesToAdd);
        parent.TrustedHardware.AddRange(entitiesToAdd);
        await Context.SaveChangesAsync();

        return GetSuccessfulResult();
    }
}
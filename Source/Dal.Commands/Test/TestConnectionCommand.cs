using CleanModels.Commands.Base;
using Dal.Commands.Base;
using Microsoft.EntityFrameworkCore;

namespace Dal.Commands.Test;

public class TestConnectionCommand : DbCommandBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestConnectionCommand"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public TestConnectionCommand(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult> ExecuteCoreAsync()
    {
        await Context.SaveChangesAsync();

        return GetSuccessfulResult();
    }
}
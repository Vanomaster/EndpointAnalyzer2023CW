using CleanModels;
using CleanModels.Commands.Base;
using CleanModels.Queries.Base;

namespace Server.Services.Base;

public interface IScheduleService<TModel>
{
    Task<QueryResult<List<TModel>>> GetAsync(PageModel pageModel);

    Task<CommandResult> AddOrUpdateAsync(TModel model);

    Task<CommandResult> RunAsync(string model);

    Task<CommandResult> DisableAsync(TModel model);

    Task<CommandResult> EnableAsync(TModel model);

    Task<CommandResult> RemoveAsync(IEnumerable<TModel> model);
}
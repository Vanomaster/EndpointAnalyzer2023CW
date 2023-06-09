using CleanModels;
using CleanModels.Commands.Base;
using CleanModels.Queries.Base;

namespace Server.Services.Base;

public interface IDbMultipleService<TModel>
{
    Task<QueryResult<TModel>> GetOneAsync(string parentName);

    Task<QueryResult<List<TModel>>> GetPageAsync(PageModel pageModel);

    // Task<QueryResult<List<TModel>>> GetAllAsync();

    Task<QueryResult<List<string>>> GetAllNamesAsync();

    Task<QueryResult<List<TModel>>> GetExistAsync(List<TModel> model);

    Task<CommandResult> AddAsync(List<TModel> model);

    Task<CommandResult> UpdateAsync(List<TModel> model);

    // Task<CommandResult> AddOrUpdateAsync(TModel model);

    Task<CommandResult> RemoveAsync(IEnumerable<TModel> model);
}
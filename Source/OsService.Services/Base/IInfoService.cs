using CleanModels;
using CleanModels.Queries.Base;

namespace OsService.Services.Base;

public interface IInfoService<TModel>
{
    Task<QueryResult<TModel>> GetAsync<TQuery>()
        where TQuery : IQuery<string>;

    public Task<QueryResult<List<ConfigurationVerification>>> GetAsync<TQuery>(List<string> verificationCommands)
        where TQuery : IQuery<List<string>, List<ConfigurationVerification>>;
}
using CleanModels;
using CleanModels.Queries.Base;
using Dal.Entities;

namespace Server.Services.Base;

public interface IAnalysisResultService
{
    public Task<QueryResult<List<AnalysisResult>>> Get(PageModel pageModel);
}
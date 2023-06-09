using CleanModels.Analysis;
using CleanModels.Commands.Base;

namespace Server.Services.Base;

public interface IAnalysisService
{
    public Task<CommandResult> AnalyzeAsync(AnalysisModel analysisModel);
}
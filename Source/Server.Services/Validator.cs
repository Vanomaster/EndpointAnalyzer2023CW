using System.Text;
using CleanModels.Commands.Base;

namespace Server.Services;

public static class Validator
{
    public static CommandResult ValidateAnalyzerNames(List<string> analyzerNames)
    {
        var validationErrors = new StringBuilder();
        foreach (string analyzerName in analyzerNames)
        {
            var analyzerTypeQueryResult = AnalyzerProvider.GetAnalyzerTypeByName(analyzerName);
            if (!analyzerTypeQueryResult.IsSuccessful)
            {
                validationErrors.Append(analyzerTypeQueryResult.ErrorMessage);
            }
        }

        if (!string.IsNullOrWhiteSpace(validationErrors.ToString()))
        {
            return new CommandResult(validationErrors.ToString());
        }

        return new CommandResult();
    }
}
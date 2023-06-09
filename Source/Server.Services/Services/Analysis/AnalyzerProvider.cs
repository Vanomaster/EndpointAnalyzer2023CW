using Analyzers;
using CleanModels.Queries.Base;

namespace Server.Services;

/// <summary>
/// Analyzer provider.
/// </summary>
public static class AnalyzerProvider
{
    private static readonly Dictionary<string, Type> AnalyzerTypeByNames = new ()
    {
        { @"Анализ конфигураций", typeof(ConfigurationAnalyzer) },
        { @"Анализ устройств", typeof(HardwareTrustAnalyzer) },
        { @"Анализ ПО", typeof(SoftwareTrustAnalyzer) },
        { @"Анализ обновлений ПО", typeof(SoftwareUpdateAnalyzer) },
    };

    public static QueryResult<Type> GetAnalyzerTypeByName(string analyzerName)
    {
        if (!AnalyzerTypeByNames.TryGetValue(analyzerName, out var analyzerType))
        {
            return new QueryResult<Type>(@$"Проверки {analyzerName} не существует.");
        }

        return new QueryResult<Type>(analyzerType);
    }
}
namespace Analyzers.Base;

/// <summary>
/// Analyzer.
/// </summary>
/// <typeparam name="TModel">Model of analyze.</typeparam>
public interface IAnalyzer<in TModel>
{
    /// <summary>
    /// Analyze.
    /// </summary>
    /// <param name="model">Analyze model.</param>
    /// <returns>Analyze result model.</returns>
    Task Analyze(TModel model);
}
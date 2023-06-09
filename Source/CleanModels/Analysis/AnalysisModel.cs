namespace CleanModels.Analysis;

public class AnalysisModel
{
    public string PcIp { get; set; }

    public string BenchmarkName { get; set; }

    public List<string> AnalyzerNames { get; set; } = new ();
}
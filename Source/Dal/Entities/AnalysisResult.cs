namespace Dal.Entities;

public class AnalysisResult : IEntity
{
    public Guid Id { get; set; }

    public string PcName { get; set; }

    public string BenchmarkName { get; set; }

    public string AnalyzerName { get; set; }

    public byte[] Text { get; set; }

    public DateTime DateTime { get; set; }
}
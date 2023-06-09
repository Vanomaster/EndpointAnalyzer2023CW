using CleanModels.Network;

namespace CleanModels.Schedule;

public class AnalysisScheduleRecord
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Host Host { get; set; }

    public string BenchmarkName { get; set; } = null!;

    public List<string> AnalyzerNames { get; set; } = new ();

    public string Recurrence { get; set; } = null!;

    public bool Enabled { get; set; }

    public DateTime NextAnalysisDateTime { get; set; }
}
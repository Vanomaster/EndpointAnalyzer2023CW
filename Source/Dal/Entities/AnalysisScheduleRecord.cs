namespace Dal.Entities;

public class AnalysisScheduleRecord : INamedEntity // unique PcIp + BenchmarkName + AnalyzerName + Recurrence
{
    public Guid Id { get; set; }

    public string Name { get; set; } // unique case insensitive name or use Id for it

    public string PcIp { get; set; } = null!;

    public string BenchmarkName { get; set; } = null!;

    public string AnalyzerNames { get; set; } = null!;

    public string Recurrence { get; set; } = null!; // cron expression

    public bool Enabled { get; set; } // when setting false value then delete from schedule db

    // public DateTime DateTime { get; set; } // maybe better if will be string cron expression instead this 3 props
    //
    // public DateOnly DateInterval { get; set; } // interval (recur every x day)
    //
    // public TimeOnly TimeInterval { get; set; } // interval (recur every x time (minute, hour))
}
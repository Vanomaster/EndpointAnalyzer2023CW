using Dal.Entities;

namespace Dal.EqualityComparers;

/// <inheritdoc />
public sealed class AnalysisScheduleRecordComparer : IEqualityComparer<AnalysisScheduleRecord>
{
    /// <inheritdoc/>
    public bool Equals(AnalysisScheduleRecord first, AnalysisScheduleRecord second)
    {
        return first.Name.ToLower() == second.Name.ToLower() ||
               first.PcIp + first.BenchmarkName + first.AnalyzerNames ==
               second.PcIp + second.BenchmarkName + second.AnalyzerNames;
    }

    /// <inheritdoc/>
    public int GetHashCode(AnalysisScheduleRecord obj)
    {
        return obj.Name.GetHashCode();
    }
}
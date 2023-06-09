using Dal.Entities;

namespace Dal.EqualityComparers;

/// <inheritdoc />
public sealed class TrustedHardwareBenchmarkComparer : IEqualityComparer<TrustedHardwareBenchmark>
{
    /// <inheritdoc/>
    public bool Equals(TrustedHardwareBenchmark first, TrustedHardwareBenchmark second)
    {
        return first.Name == second.Name;
    }

    /// <inheritdoc/>
    public int GetHashCode(TrustedHardwareBenchmark obj)
    {
        return obj.Name.GetHashCode();
    }
}
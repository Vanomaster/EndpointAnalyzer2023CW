using Dal.Entities;

namespace Dal.EqualityComparers;

/// <inheritdoc />
public sealed class TrustedSoftwareBenchmarkComparer : IEqualityComparer<TrustedSoftwareBenchmark>
{
    /// <inheritdoc/>
    public bool Equals(TrustedSoftwareBenchmark first, TrustedSoftwareBenchmark second)
    {
        return first.Name == second.Name;
    }

    /// <inheritdoc/>
    public int GetHashCode(TrustedSoftwareBenchmark obj)
    {
        return obj.Name.GetHashCode();
    }
}
using Dal.Entities;

namespace Dal.EqualityComparers;

/// <inheritdoc />
public sealed class BenchmarkComparer : IEqualityComparer<Benchmark>
{
    /// <inheritdoc/>
    public bool Equals(Benchmark first, Benchmark second)
    {
        return first.Name == second.Name;
    }

    /// <inheritdoc/>
    public int GetHashCode(Benchmark obj)
    {
        return obj.Name.GetHashCode();
    }
}
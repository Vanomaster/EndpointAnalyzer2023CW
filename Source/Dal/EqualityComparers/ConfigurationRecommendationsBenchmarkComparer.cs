using Dal.Entities;

namespace Dal.EqualityComparers;

/// <inheritdoc />
public sealed class ConfigurationRecommendationsBenchmarkComparer : IEqualityComparer<ConfigurationRecommendationsBenchmark>
{
    /// <inheritdoc/>
    public bool Equals(ConfigurationRecommendationsBenchmark first, ConfigurationRecommendationsBenchmark second)
    {
        return first.Name == second.Name;
    }

    /// <inheritdoc/>
    public int GetHashCode(ConfigurationRecommendationsBenchmark obj)
    {
        return obj.Name.GetHashCode();
    }
}
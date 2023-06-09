using Dal.Entities;

namespace Dal.EqualityComparers;

/// <inheritdoc />
public sealed class ConfigurationRecommendationComparer : IEqualityComparer<ConfigurationRecommendation>
{
    /// <inheritdoc/>
    public bool Equals(ConfigurationRecommendation first, ConfigurationRecommendation second)
    {
        return first.Name == second.Name ||
               (first.VerificationResult == second.VerificationResult &&
                first.Configuration.Name == second.Configuration.Name);
    }

    /// <inheritdoc/>
    public int GetHashCode(ConfigurationRecommendation obj)
    {
        return obj.Name.GetHashCode();
    }
}
using Dal.Entities;

namespace Dal.EqualityComparers;

/// <inheritdoc />
public sealed class ConfigurationComparer : IEqualityComparer<Configuration>
{
    /// <inheritdoc/>
    public bool Equals(Configuration first, Configuration second)
    {
        return first.Name == second.Name;
    }

    /// <inheritdoc/>
    public int GetHashCode(Configuration obj)
    {
        return obj.Name.GetHashCode();
    }
}
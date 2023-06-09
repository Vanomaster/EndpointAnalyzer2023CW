using Dal.Entities;

namespace Dal.EqualityComparers;

/// <inheritdoc />
public sealed class TrustedSoftwareComparer : IEqualityComparer<TrustedSoftware>
{
    /// <inheritdoc/>
    public bool Equals(TrustedSoftware first, TrustedSoftware second)
    {
        return first.Name == second.Name && first.Version == second.Version;
    }

    /// <inheritdoc/>
    public int GetHashCode(TrustedSoftware obj)
    {
        string text = obj.Name + obj.Version;

        return text.GetHashCode();
    }
}
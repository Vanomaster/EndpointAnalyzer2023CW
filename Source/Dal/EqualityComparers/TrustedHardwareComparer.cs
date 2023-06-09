using Dal.Entities;

namespace Dal.EqualityComparers;

/// <inheritdoc />
public sealed class TrustedHardwareComparer : IEqualityComparer<TrustedHardware>
{
    /// <inheritdoc/>
    public bool Equals(TrustedHardware first, TrustedHardware second)
    {
        return first.Name == second.Name || first.HardwareId == second.HardwareId;
    }

    /// <inheritdoc/>
    public int GetHashCode(TrustedHardware obj)
    {
        return obj.HardwareId.GetHashCode();
    }
}
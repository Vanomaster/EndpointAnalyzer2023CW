namespace Dal.Entities;

/// <summary>
/// Entity.
/// </summary>
public interface INamedEntity : IEntity
{
    /// <summary>
    /// Name.
    /// </summary>
    public string Name { get; set; }
}
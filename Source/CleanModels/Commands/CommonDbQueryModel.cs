namespace CleanModels.Commands;

public class CommonDbQueryModel<TModel>
{
    public CommonDbQueryModel(
        List<TModel?> models,
        string[] uniqueValuePropertyNames,
        string? propertyToIncludePath = null)
    {
        if (propertyToIncludePath == null && uniqueValuePropertyNames.Any(name => name.Contains('.')))
        {
            throw new ArgumentException("The propertyPath is null. " +
                                        "The propertyPath need to be not null " +
                                        "when using dot (.) in uniqueValuePropertyNames.");
        }

        Models = models;
        UniqueValuePropertyNames = uniqueValuePropertyNames;
        PropertyToIncludePath = propertyToIncludePath;
    }

    public List<TModel?> Models { get; }

    public string[] UniqueValuePropertyNames { get; }

    public string? PropertyToIncludePath { get; }
}
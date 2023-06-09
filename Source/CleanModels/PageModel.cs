namespace CleanModels;

public class PageModel
{
    public int PageNumber { get; set; }

    public string? SearchPhrase { get; set; }

    public string? SortingPropertyName { get; set; }

    public bool? SortingOrderIsAscending { get; set; }

    public string? ParentName { get; set; }
}
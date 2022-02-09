namespace IntelligenceReporting.WebApp.Structs;

/// <summary>The order by used for queries</summary>
public struct OrderBy
{
    /// <summary>The name of the column</summary>
    public string ColumnName { get; set; }

    /// <summary>Whether the query is in descending order</summary>
    public bool IsDescending { get; set; }

    /// <summary>The text to display</summary>
    public string Description { get; set; }

    /// <summary>Whether the item is the default selection</summary>
    public bool IsSelected { get; set; }

    /// <summary>Constructor where column name and description are the same</summary>
    public OrderBy(string columnName, bool isDescending = false, bool isSelected = false) :
        this(columnName, columnName, isDescending, isSelected) {}

    /// <summary>Constructor where column name and description are different</summary>
    public OrderBy(string columnName, string description, bool isDescending = false, bool isSelected = false)
    {
        ColumnName = columnName;
        Description = description;
        IsDescending = isDescending;
        IsSelected = isSelected;
    }
}

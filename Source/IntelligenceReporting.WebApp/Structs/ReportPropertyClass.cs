namespace IntelligenceReporting.WebApp.Structs;

/// <summary>The report property class</summary>
public struct ReportPropertyClass
{
    /// <summary>The ID of the report property class</summary>
    public int Id { get; set; }

    /// <summary>The name of the report property class</summary>
    public string Name { get; set; }

    /// <summary>Constructor where column name and description are different</summary>
    public ReportPropertyClass(int id, string name)
    {
        Id = id;
        Name = name;
    }
}

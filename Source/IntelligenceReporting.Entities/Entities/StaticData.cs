namespace IntelligenceReporting.Entities;

/// <summary>A bunch of stuff that doesn't change much, and is always needed on the UI</summary>
public class StaticData
{
    /// <summary>Brands</summary>
    public Brand[] Brands { get; set; } = Array.Empty<Brand>();

    /// <summary>Agent roles</summary>
    public Role[] Roles { get; set; } = Array.Empty<Role>();

    /// <summary>Land area types</summary>
    public LandAreaType[] LandAreaTypes { get; set; } = Array.Empty<LandAreaType>();

    /// <summary>Sources</summary>
    public Source[] Sources { get; set; } = Array.Empty<Source>();

    /// <summary>States</summary>
    public State[] States { get; set; } = Array.Empty<State>();

    /// <summary>Countries</summary>
    public Country[] Countries { get; set; } = Array.Empty<Country>();

    /// <summary>Statuses</summary>
    public Status[] Statuses { get; set; } = Array.Empty<Status>();

    /// <summary>Report Method of Sales</summary>
    public ReportMethodOfSale[] ReportMethodOfSales { get; set; } = Array.Empty<ReportMethodOfSale>();
}
using IntelligenceReporting.WebApp.Structs;

namespace IntelligenceReporting.WebApp.Services;

public interface IFilterSelectionService
{
    Tracked<int> BrandId { get; }
    Tracked<int> MultiOfficeId { get; }
    Tracked<int> OfficeId { get; }
    Tracked<int> StaffId { get; }
    Tracked<int> CountryId { get; }
    Tracked<int> StateId { get; }
    Tracked<int> ReportMethodOfSaleId { get; }
    TrackedCollection<int> ReportPropertyClassIds { get; }
    Tracked<OrderBy> OrderBy { get; }
}


public class FilterSelectionService : IFilterSelectionService
{
    public Tracked<int> BrandId { get; } = new();
    public Tracked<int> MultiOfficeId { get; } = new();
    public Tracked<int> OfficeId { get; } = new();
    public Tracked<int> StaffId { get; } = new();
    public Tracked<int> CountryId { get; } = new();
    public Tracked<int> StateId { get; } = new();
    public Tracked<int> ReportMethodOfSaleId { get; } = new();
    public TrackedCollection<int> ReportPropertyClassIds { get; } = new();
    public Tracked<OrderBy> OrderBy { get; } = new();
}

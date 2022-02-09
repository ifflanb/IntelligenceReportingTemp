using IntelligenceReporting.Extensions;

namespace IntelligenceReporting.Parameters;

/// <summary>Parameters for calculating earnings</summary>
public class CalculateEarningsParameters
{
    /// <summary>The start date to calculate from</summary>
    public DateTime StartDate { get; set; } = DateTime.Now.StartOfMonth();
        
    /// <summary>The end date to calculate up to</summary>
    public DateTime EndDate { get; set; } = DateTime.Now.EndOfMonth();
        
    /// <summary>Restrict calculation to an office</summary>
    public int? OfficeId { get; set; }

    /// <summary>Restrict calculation to a staff member</summary>
    public int? StaffId { get; set; }
}
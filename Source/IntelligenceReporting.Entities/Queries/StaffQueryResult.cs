using System.ComponentModel.DataAnnotations;

namespace IntelligenceReporting.Queries;

/// <summary>A staff query result</summary>
public class StaffQueryResult
{
    /// <summary>The id of the office</summary>
    public int OfficeId { get; set; }
    
    /// <summary>The name of the office</summary>
    public string OfficeName { get; set; } = "";

    /// <summary>The id of the staff member</summary>
    [Key]
    public int StaffId { get; set; }

    /// <summary>The name of the staff member</summary>
    public string StaffName { get; set; } = "";

    /// <summary>The country of the staff member</summary>
    public int? CountryId { get; set; }

    /// <summary>The state of the staff member</summary>
    public int? StateId { get; set; }

    /// <summary>The multi-office group (franchise) of the staff member</summary>
    public int? MultiOfficeId { get; set; }

    /// <summary>Staff is active</summary>
    public bool IsActive { get; set; }
}
namespace IntelligenceReporting.Entities;

/// <summary>A staff member</summary>
public class Staff : SynchronizedEntity
{
    /// <summary>The modify date time of this staff</summary>
    /// <remarks>This is used to sync with the source database</remarks>
    public DateTimeOffset LastUpdated { get; set; }

    /// <summary>The office this staff belongs to</summary>
    public int OfficeId { get; set; }

    /// <summary>The full name of the staff member</summary>
    public string StaffName { get; set; } = "";

    /// <summary>This staff is active</summary>
    public bool IsActive { get; set; }
}
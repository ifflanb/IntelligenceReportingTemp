namespace IntelligenceReporting.Entities;

/// <summary>A listing/appraisal agent</summary>
public class SaleLifeEarnings : Entity
{
    /// <summary>The office</summary>
    public int OfficeId { get; set; }

    /// <summary>The staff member</summary>
    public int StaffId { get; set; }

    /// <summary>The agent's role</summary>
    public RoleId RoleId { get; set; }

    /// <summary>The split percent</summary>
    public decimal SplitPercent { get; set; }

    /// <summary>The gross commission earned for the office</summary>
    public decimal GrossSplit { get; set; }

    /// <summary>The net commission paid to the agent</summary>
    public decimal NetSplit { get; set; }
}
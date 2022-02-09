namespace IntelligenceReporting.Entities;

/// <summary>Sale life status</summary>
public class Status : Entity
{
    /// <summary>Name of the status</summary>
    public string StatusName { get; set; } = "";
}


/// <summary>Status ids</summary>
public enum StatusId
{
    /// <summary>Prospect / Not currently listed</summary>
    Prospect = 1,
    /// <summary>Appraisal</summary>
    Appraisal = 2 ,
    /// <summary>Listing</summary>
    Listing = 3 ,
    /// <summary>Conditional</summary>
    Conditional = 4 ,
    /// <summary>Unconditional</summary>
    Unconditional = 5 ,
    /// <summary>Settled</summary>
    Settled = 6 ,
    /// <summary>Rental listing</summary>
    ManagementListing = 7 ,
    /// <summary>Withdrawn listing</summary>
    WithdrawnListing = 8 ,
    /// <summary>Withdrawn appraisal</summary>
    WithdrawnAppraisal = 9 ,
    /// <summary>Fallen sale</summary>
    FallenSale = 10
}
namespace IntelligenceReporting.Entities;

/// <summary>Sale life Report Method of Sale</summary>
public class ReportMethodOfSale : Entity
{
    /// <summary>Name of the Report Method of Sale</summary>
    public string ReportMethodOfSaleName { get; set; } = "";
}

/// <summary>Ids of report method of sales</summary>
public enum ReportMethodOfSaleId
{
    /// <summary>Auction</summary>
    Auction = 1,
    /// <summary>Tender</summary>
    Tender = 2,
    /// <summary>Exclusive</summary>
    Exclusive = 3,
    /// <summary>General</summary>
    General = 4
}
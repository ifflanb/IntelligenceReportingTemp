
namespace IntelligenceReporting.Entities
{
    /// <summary>The appraisal through sale of a property</summary>
    public class SaleLife : SynchronizedEntity
    {
        /// <summary>The office this belongs to (our id)</summary>
        public int OfficeId { get; set; }

        /// <summary>The address of the property</summary>
        public string Address { get; set; } = "";

        /// <summary>The modify date time of this sale life</summary>
        /// <remarks>This is used to sync with the source database</remarks>
        public DateTimeOffset LastUpdated { get; set; }

        /// <summary>The status of this sale life</summary>
        public StatusId StatusId { get; set; }

        /// <summary>The report method of sale id of this sale life</summary>
        public ReportMethodOfSaleId ReportMethodOfSaleId { get; set; }

        /// <summary>The land area type eg. square meter, hectare,..</summary>
        public LandAreaTypeId? LandAreaTypeId { get; set; }

        /// <summary>The land area amount</summary>
        public decimal? LandArea { get; set; }

        /// <summary>Is this a unit title sale</summary>
        public bool IsUnitTitle { get; set; }

        /// <summary>The appraisal date</summary>
        public DateTime? AppraisalDate { get; set; }

        /// <summary>The listing authority date</summary>
        public DateTime? ListingDate { get; set; }

        /// <summary>The auction date</summary>
        public DateTime? AuctionDate { get; set; }

        /// <summary>The conditional date of the sale</summary>
        public DateTime? ConditionalDate { get; set; }

        /// <summary>The date the sale went/goes unconditional</summary>
        public DateTime? UnconditionalDate { get; set; }

        /// <summary>The date the sale is to settle</summary>
        public DateTime? SettlementDate { get; set; }

        /// <summary>The date the settlement was actioned</summary>
        public DateTimeOffset? SettlementActioned { get; set; }

        /// <summary>The date the sale administration completed - everyone's been paid</summary>
        public DateTime? AdminComplete { get; set; }

        /// <summary>The datetime the sale admin was completed</summary>
        public DateTimeOffset? AdminCompleteActioned { get; set; }

        /// <summary>The gross commission paid by the vendor</summary>
        public decimal? GrossCommission { get; set; }

        /// <summary>The Harcourts foundation fee paid by the office</summary>
        public decimal? FoundationContribution { get; set; }

        /// <summary>The Office's gross income after paying other offices and external fees</summary>
        public decimal? OfficeGrossIncome { get; set; }

        /// <summary>The listing/appraisal agents</summary>
        public IList<SaleLifeAgent> Agents { get; } = new List<SaleLifeAgent>();

        /// <summary>The agent earnings from the sale</summary>
        public IList<SaleLifeEarnings> Earnings { get; } = new List<SaleLifeEarnings>();

        /// <summary>The listing type id of this sale life</summary>
        public ListingTypeId ListingTypeId { get; set; }

        /// <summary>The property class</summary>
        public PropertyClassId PropertyClassId { get; set; }

        /// <summary><inheritdoc cref="object.ToString"/></summary>
        public override string ToString() => $"SaleLife Id={Id}, ExternalId={ExternalId}, OfficeId={OfficeId} {Address} {StatusId}";
    }

    /// <summary>Ids of listing types</summary>
    public enum ListingTypeId
    {
        /// <summary>Sale</summary>
        Sale = 1,
        /// <summary>Lease</summary>
        Lease = 2,
        /// <summary>Both</summary>
        Both = 3
    }
	
    /// <summary>Ids of report method of sales</summary>
    public enum PropertyClassId
    {
        /// <summary>Auction</summary>
        Residential = 1,
        /// <summary>Commercial</summary>
        Commercial = 2,
        /// <summary>Business</summary>
        Business = 3,
        /// <summary>Rural</summary>
        Rural = 4,
        /// <summary>Holiday Rental</summary>
        HolidayRental = 5,
        /// <summary>Land</summary>
        Land = 6,
        /// <summary>Livestock</summary>
        Livestock = 7,
        /// <summary>Clearing Sales</summary>
        ClearingSales = 8
    }

    public enum ReportPropertyClassId
    {
        /// <summary>﻿﻿Residential﻿ and Land</summary>
        Residential﻿Land = 1,
        /// <summary>Rural</summary>
        Rural = 2,
        /// <summary>﻿Rural (Over 40 acres)</summary>
        RuralOver40Acres = 3,
        /// <summary>﻿Rural (Over 20 hectares)</summary>
        RuralOver20Hectares = 4,
        /// <summary>Commercial</summary>
        Commercial = 5,
        /// <summary>Commercial Lease</summary>
        CommercialLease = 6,
        /// <summary>Business﻿</summary>
        Business = 7
    }
}

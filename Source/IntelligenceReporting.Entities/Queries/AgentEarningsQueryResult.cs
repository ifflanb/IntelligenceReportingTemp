using Microsoft.EntityFrameworkCore;

namespace IntelligenceReporting.Queries
{
    /// <summary>Agent earnings query results for a single agent</summary>
    [Keyless]
    public class AgentEarningsQueryResult
    {
        /// <summary>The id of the agent's office</summary>
        public int OfficeId { get; set; }

        /// <summary>The name of the agent's office</summary>
        public string OfficeName { get; set; } = "";

        /// <summary>The staff id of the agent</summary>
        public int StaffId { get; set; }

        /// <summary>The full name of the agent</summary>
        public string StaffName { get; set; } = "";

        /// <summary>The brand id of the agent's office</summary>
        public int? BrandId { get; set; }

        /// <summary>The brand name of the agent's office</summary>
        public string? BrandName { get; set; } = "";

        /// <summary>The state id of the agent's office</summary>
        public int? StateId { get; set; }

        /// <summary>The state name of the agent's office</summary>
        public string? StateName { get; set; } = "";

        /// <summary>The country id of the agent's office</summary>
        public int? CountryId { get; set; }

        /// <summary>The country name of the agent's office</summary>
        public string? CountryName { get; set; } = "";

        /// <summary>The id of the multi-office the agent's office belongs to (e.g. Cooper &amp; Co, Grenadier)</summary>
        public int? MultiOfficeId { get; set; }

        /// <summary>The name of the multi-office the agent's office belongs to (e.g. Cooper &amp; Co, Grenadier)</summary>
        public string? MultiOfficeName { get; set; }

        /// <summary>The number of sale lives for this agent with an appraisal date within this period</summary>
        public decimal Appraisals { get; set; }

        /// <summary>The number of sale lives for this agent and listing date within this period</summary>
        public decimal Listings { get; set; }

        /// <summary>The number of sale lives for this agent and auction date within this period</summary>
        public decimal Auctions { get; set; }

        /// <summary>The number of sale lives for this agent with a conditional date within this period</summary>
        public decimal Conditionals { get; set; }

        /// <summary>The number of sale lives for this agent with an unconditional date within this period</summary>
        public decimal Unconditionals { get; set; }

        /// <summary>The number of sale lives for this agent that were settled date within this period</summary>
        public decimal Settlements { get; set; }

        /// <summary>The sum of commission received for sale lives for this agent and settled date within this period</summary>
        public decimal Commission { get; set; }

        /// <summary>The sum of listing units for sale lives for this agent and authority date within this period - divided by the number of listing agents</summary>
        public decimal ListingUnits { get; set; }

        /// <summary>The number of enquiries for this agent and date within this period</summary>
        public decimal Enquiries { get; set; }

        /// <summary>The number of inspections for this agent and date within this period</summary>
        public decimal Inspections { get; set; }

        /// <summary>Average days on market (conditional date - listing date)</summary>
        public decimal AverageDaysOnMarket { get; set; }

        /// <summary>Sales per listing (conditional / listings)</summary>
        public decimal SalesPerListing { get; set; }
        
        /// <summary>Listings per appraisal ( listings / appraisals) </summary>
        public decimal ListingsPerAppraisal { get; set; }

        /// <summary>Settlements per sale (settled / conditional)</summary>
        public decimal SettlementsPerSale { get; set; }
    }
}

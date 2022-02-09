using IntelligenceReporting.Extensions;

namespace IntelligenceReporting.Queries
{
    /// <summary>Agent earnings query parameters</summary>
    public class AgentEarningsQueryParameters: QueryParameters
    {
        /// <summary>The start date</summary>
        public DateTime StartDate { get; set; } = DateTime.Now.StartOfMonth();
        
        /// <summary>The end date</summary>
        public DateTime EndDate { get; set; } = DateTime.Now.EndOfMonth();
        
        /// <summary>Restrict results to an office</summary>
        public int? OfficeId { get; set; }

        /// <summary>Restrict results to a staff member</summary>
        public int? StaffId { get; set; }
        
        /// <summary>Restrict results to a brand</summary>
        public int? BrandId { get; set; }

        /// <summary>Restrict results to a state</summary>
        public int? StateId { get; set; }

        /// <summary>Restrict results to a country</summary>
        public int? CountryId { get; set; }

        /// <summary>Restrict results to a multi-office grouping</summary>
        public int? MultiOfficeId { get; set; }

        /// <summary>Restrict results to a report method of sale</summary>
        public int? ReportMethodOfSaleId { get; set; }
		
        /// <summary>Restrict results to one or more report property classes</summary>
        public List<int>? ReportPropertyClassIds { get; set; }
    }
}

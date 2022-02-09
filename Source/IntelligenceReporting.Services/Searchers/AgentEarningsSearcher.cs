using System.Data.Common;
using IntelligenceReporting.Databases;
using IntelligenceReporting.Queries;
using Microsoft.Extensions.Logging;

namespace IntelligenceReporting.Searchers;

public class AgentEarningsSearcher : RawSqlSearcher<AgentEarningsQueryParameters, AgentEarningsQueryResult, AgentEarningsPagedResults>, IAgentEarningsSearcher
{
    public AgentEarningsSearcher(IntelligenceReportingDbContext dbContext, ILogger<AgentEarningsSearcher> logger)
        : base(dbContext, logger) { }

    protected override string BuildSqlCommonTableExpressions(AgentEarningsQueryParameters parameters)
    {
        var saleLifeFilter = ""
            .AppendSqlWhere(parameters.ReportMethodOfSaleId.HasValue, "sl.ReportMethodOfSaleId = " + parameters.ReportMethodOfSaleId);

        var staffFilter = ""
            .AppendSqlWhere(parameters.BrandId.HasValue, "b.Id = " + parameters.BrandId)
            .AppendSqlWhere(parameters.CountryId.HasValue, "c.Id = " + parameters.CountryId)
            .AppendSqlWhere(parameters.StateId.HasValue, "st.Id = " + parameters.StateId)
            .AppendSqlWhere(parameters.MultiOfficeId.HasValue, "mo.Id = " + parameters.MultiOfficeId)
            .AppendSqlWhere(parameters.OfficeId.HasValue, "o.Id = " + parameters.OfficeId)
            .AppendSqlWhere(parameters.StaffId.HasValue, "s.Id = " + parameters.StaffId);

        var sql = $@"
DECLARE @startDate Date = {parameters.StartDate.FormatSql()}
DECLARE @endDate Date = {parameters.EndDate.FormatSql()}

; WITH 

-- sale lives
sl AS (
    SELECT sl.*
    FROM SaleLife sl
{saleLifeFilter}
),

-- agents - filtered
s AS (
    Select
        b.Id BrandId,
        b.BrandName,
        c.Id CountryId,
        c.CountryName,
        st.Id StateId,
        st.StateName,
        o.MultiOfficeId,
        mo.MultiOfficeName,
        o.Id OfficeId,
        o.OfficeName,
        s.Id StaffId,
        s.StaffName
    FROM Staff s
    JOIN Office o ON o.Id = s.OfficeId
    LEFT JOIN Brand b ON b.Id = o.BrandId
    LEFT JOIN State st ON st.Id = b.Id
    LEFT JOIN Country c ON c.id = st.CountryId
    LEFT JOIN MultiOffice mo on mo.Id = o.MultiOfficeId
{staffFilter}
),

-- sale life agent
sla AS (
    SELECT sl.*, sla.StaffId, 
        (SELECT COUNT(*) FROM SaleLifeAgent WHERE SaleLifeId = sl.Id) AgentCount
    FROM s
    JOIN sl on sl.OfficeId = s.OfficeId
    -- join using clustered index
    JOIN SaleLifeAgent sla ON sla.OfficeId = s.OfficeId AND sla.StaffId = s.StaffId AND sla.SaleLifeId = sl.Id
),

-- sale life earnings
sle AS (
    SELECT sl.*, sle.StaffId, sle.RoleId, sle.SplitPercent, sle.GrossSplit, sle.NetSplit,
        (SELECT COUNT(*) FROM SaleLifeEarnings WHERE SaleLifeId = sl.Id) AgentCount
    FROM s
    JOIN sl on sl.OfficeId = s.OfficeId
    -- join using clustered index
    JOIN SaleLifeEarnings sle ON sle.OfficeId = s.OfficeId AND sle.StaffId = s.StaffId AND sle.SaleLifeId = sl.Id
)
";
        return sql;
    }

    // ToDo: Make the field selection dynamic according to the search parameters and view requested
    // - there's no sense bulking up the query results with brand, country, state multioffice and office or staff for a single agent
    protected override string BuildDetailSql(AgentEarningsQueryParameters parameters) => @$"
SELECT
    s.BrandId,
    s.BrandName,
    s.CountryId,
    s.CountryName,
    s.StateId,
    s.StateName,
    s.MultiOfficeId,
    s.MultiOfficeName,
    s.OfficeId,
    s.OfficeName,
    s.StaffId,
    s.StaffName,
{SqlAggregations}
GROUP BY s.OfficeId, s.OfficeName, s.StaffId, s.StaffName, s.BrandId, s.BrandName, s.StateId, s.StateName, s.CountryId, s.CountryName, s.MultiOfficeId, s.MultiOfficeName
";

    protected override AgentEarningsQueryResult ReadDetail(DbDataReader reader)
    {
        var result = new AgentEarningsQueryResult();
        var field = 0;
        result.BrandId = reader.GetNullableInt32(field++);
        result.BrandName = reader.GetNullableString(field++);
        result.CountryId = reader.GetNullableInt32(field++);
        result.CountryName = reader.GetNullableString(field++);
        result.StateId = reader.GetNullableInt32(field++);
        result.StateName = reader.GetNullableString(field++);
        result.MultiOfficeId = reader.GetNullableInt32(field++);
        result.MultiOfficeName = reader.GetNullableString(field++);
        result.OfficeId = reader.GetInt32(field++);
        result.OfficeName = reader.GetString(field++);
        result.StaffId = reader.GetInt32(field++);
        result.StaffName = reader.GetString(field++);
        ReadAggregations(reader, result, ref field);
        return result;
    }

    protected override string GetDefaultOrderBys()
        => "BrandName,CountryName,StateName,MultiOfficeName,OfficeName,StaffName";

    // ToDo: aggregations should also be dynamic depending on the view - only query and return results we're going to display
    private static string SqlAggregations => @"
    (SELECT COUNT(*) FROM sla WHERE sla.StaffId = s.StaffId AND sla.AppraisalDate between @startDate and @endDate) Appraisals,
    (SELECT COUNT(*) FROM sla WHERE sla.StaffId = s.StaffId AND sla.ListingDate between @startDate and @endDate) Listings,
    (SELECT COUNT(*) FROM sla WHERE sla.StaffId = s.StaffId AND sla.AuctionDate between @startDate and @endDate) Auctions,
    (SELECT COUNT(*) FROM sle WHERE sle.StaffId = s.StaffId AND ConditionalDate between @startDate and @endDate) Conditionals,
    (SELECT COUNT(*) FROM sle WHERE sle.StaffId = s.StaffId AND UnconditionalDate between @startDate and @endDate) Unconditionals,
    (SELECT COUNT(*) FROM sle WHERE sle.StaffId = s.StaffId AND SettlementDate between @startDate and @endDate) Settlements,
    (SELECT ISNULL(SUM(sle.NetSplit), 0) FROM sle WHERE sle.StaffId = s.StaffId AND SettlementDate between @startDate and @endDate) Commission,
    (SELECT ISNULL(SUM(1.0 / sla.AgentCount), 0) FROM sla WHERE sla.StaffId = s.StaffId AND sla.ListingDate between @startDate and @endDate) ListingUnits,
    0 Enquiries, -- ToDo
    0 Inspections, -- ToDo
    (
        SELECT ISNULL(AVG(CONVERT(DECIMAL(18, 10), DATEDIFF(d, ListingDate, ConditionalDate))), 0)
        FROM sle
        WHERE StaffId = s.StaffId 
        AND ConditionalDate between @startDate and @endDate
        AND ListingDate <= ConditionalDate
    ) AverageDaysOnMarket,
    (
        SELECT ISNULL(AVG(IIF(sla.ListingDate IS NOT NULL, 1.0, 0)), 0)
        FROM sla 
        WHERE sla.StaffId = s.StaffId 
        AND sla.AppraisalDate BETWEEN @startDate AND @endDate
    ) ListingsPerAppraisal

FROM s
WHERE EXISTS (
    SELECT 1 FROM sla 
    WHERE sla.StaffId = s.StaffId AND (
        sla.AppraisalDate between @startDate and @endDate
        OR sla.ListingDate between @startDate and @endDate
        OR sla.AuctionDate between @startDate and @endDate
    )
)
OR EXISTS (
    SELECT 1 FROM sle
    WHERE sle.StaffId = s.StaffId 
    AND (
        sle.ConditionalDate between @startDate and @endDate
        OR sle.UnconditionalDate between @startDate and @endDate
        OR sle.SettlementDate between @startDate and @endDate
    )
)
";

    private static void ReadAggregations(DbDataReader reader, AgentEarningsQueryResult result, ref int field)
    {
        result.Appraisals = reader.GetInt32(field++);
        result.Listings = reader.GetInt32(field++);
        result.Auctions = reader.GetInt32(field++);
        result.Conditionals = reader.GetInt32(field++);
        result.Unconditionals = reader.GetInt32(field++);
        result.Settlements = reader.GetInt32(field++);
        result.Commission = reader.GetDecimal(field++);
        result.ListingUnits = reader.GetDecimal(field++);
        result.Enquiries = reader.GetInt32(field++);
        result.Inspections = reader.GetInt32(field++);
        result.AverageDaysOnMarket = reader.GetDecimal(field++);
        result.ListingsPerAppraisal = reader.GetDecimal(field++);
        result.SalesPerListing = result.Listings == 0 ? 0 : result.Conditionals / result.Listings;
        result.SettlementsPerSale = result.Conditionals == 0 ? 0 : result.Settlements / result.Conditionals;
    }

    protected override Task OnQueryComplete(AgentEarningsPagedResults results)
    {
        // Add totals to the results
        var totalCount = results.TotalCount;
        if (totalCount == 0) return Task.CompletedTask;

        results.Total.Appraisals = results.Results.Sum(x => x.Appraisals);
        results.Total.Listings = results.Results.Sum(x => x.Listings);
        results.Total.Auctions = results.Results.Sum(x => x.Auctions);
        results.Total.Conditionals = results.Results.Sum(x => x.Conditionals);
        results.Total.Unconditionals = results.Results.Sum(x => x.Unconditionals);
        results.Total.Settlements = results.Results.Sum(x => x.Settlements);
        results.Total.Commission = results.Results.Sum(x => x.Commission);
        results.Total.ListingUnits = results.Results.Sum(x => x.ListingUnits);
        results.Total.Enquiries = results.Results.Sum(x => x.Enquiries);
        results.Total.Inspections = results.Results.Sum(x => x.Inspections);
        results.Total.AverageDaysOnMarket = results.Total.Conditionals == 0 ? 0
            : results.Results.Sum(x => x.Conditionals * x.AverageDaysOnMarket) / results.Total.Conditionals;
        results.Total.SalesPerListing = results.Total.Listings == 0 ? 0 : results.Total.Conditionals / results.Total.Listings;
        results.Total.ListingsPerAppraisal = results.Total.Appraisals == 0 ? 0 : results.Total.Listings / results.Total.Appraisals;
        results.Total.SettlementsPerSale = results.Total.Conditionals == 0 ? 0 : results.Total.Settlements / results.Total.Conditionals;

        // Add averages
        results.Average.Appraisals = results.Total.Appraisals / totalCount;
        results.Average.Listings = results.Total.Listings / totalCount;
        results.Average.Auctions = results.Total.Auctions / totalCount;
        results.Average.Conditionals = results.Total.Conditionals / totalCount;
        results.Average.Unconditionals = results.Total.Unconditionals / totalCount;
        results.Average.Settlements = results.Total.Settlements / totalCount;
        results.Average.Commission = results.Total.Commission / totalCount;
        results.Average.ListingUnits = results.Total.ListingUnits / totalCount;
        results.Average.Enquiries = results.Total.Enquiries / totalCount;
        results.Average.Inspections = results.Total.Inspections / totalCount;
        results.Average.AverageDaysOnMarket = results.Total.AverageDaysOnMarket;
        results.Average.SalesPerListing = results.Total.Listings == 0 ? 0 : results.Total.Conditionals / results.Total.Listings;
        results.Average.ListingsPerAppraisal = results.Total.Appraisals == 0 ? 0 : results.Total.Listings / results.Total.Appraisals;
        results.Average.SettlementsPerSale = results.Total.Conditionals == 0 ? 0 : results.Total.Settlements / results.Total.Conditionals;

        // It's actually cheaper to get all the results, even for all staff, than to do a second query for the totals.
        results.Results = results.Results.Skip(results.Parameters.PageSize * (results.Parameters.Page - 1)).Take(results.Parameters.PageSize).ToArray();

        // ToDo: we should cache results (e.g. for a few minutes) in case the user grabs the next page, or changes order.

        return Task.CompletedTask;
    }

}
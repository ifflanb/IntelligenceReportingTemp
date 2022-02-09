using IntelligenceReporting.Databases;
using IntelligenceReporting.Entities;
using IntelligenceReporting.Extensions;
using IntelligenceReporting.Queries;
using Microsoft.EntityFrameworkCore;

namespace IntelligenceReporting.Searchers;

public class OfficeSearcher : Searcher<OfficeQueryParameters, OfficeQueryResult, OfficePagedResults>, IOfficeSearcher
{
    public OfficeSearcher(IntelligenceReportingDbContext dbContext) : base(dbContext)
    {
    }

    protected override IQueryable<OfficeQueryResult> GetQuery(OfficeQueryParameters parameters)
    {
        var query = Context.Set<OfficeQueryResult>().FromSqlInterpolated($@"
SELECT 
    o.Id OfficeId, o.OfficeName,
    o.BrandId, b.BrandName, 
    s.CountryId,
    o.StateId, s.StateName,     
    o.MultiOfficeId, isnull(mo.MultiOfficeName, '') MultiOfficeName,
    o.IsActive 
FROM Office o
LEFT JOIN Brand b ON b.Id = o.BrandId
LEFT JOIN State s ON s.Id = o.StateId
LEFT JOIN MultiOffice mo ON mo.Id = o.MultiOfficeId
");

        if (!string.IsNullOrEmpty(parameters.OfficeName))
            query = query.Where(x => x.OfficeName.Contains(parameters.OfficeName));

        if (parameters.BrandId.HasValue)
            query = query.Where(x => x.BrandId == parameters.BrandId);

        if (parameters.CountryId.HasValue)
            query = query.Where(x => x.CountryId == parameters.CountryId);

        if (parameters.StateId.HasValue)
            query = query.Where(x => x.StateId == parameters.StateId);

        if (parameters.MultiOfficeId.HasValue)
            query = query.Where(x => x.MultiOfficeId == parameters.MultiOfficeId);

        if (parameters.IsActive.HasValue)
            query = query.Where(o => o.IsActive == parameters.IsActive);

        parameters.AddOrderBy(nameof(OfficeQueryResult.OfficeName));
        return query;
    }

    protected override IOrderedQueryable<OfficeQueryResult> GetDefaultOrderBy(IQueryable<OfficeQueryResult> query)
        => query.OrderBy(o => o.OfficeName);
}

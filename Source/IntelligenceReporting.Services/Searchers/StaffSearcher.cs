using IntelligenceReporting.Databases;
using IntelligenceReporting.Queries;
using Microsoft.EntityFrameworkCore;

namespace IntelligenceReporting.Searchers
{
    public class StaffSearcher : Searcher<StaffQueryParameters, StaffQueryResult, StaffPagedResults>, IStaffSearcher
    {
        public StaffSearcher(IntelligenceReportingDbContext dbContext) : base(dbContext) { }

        protected override IQueryable<StaffQueryResult> GetQuery(StaffQueryParameters parameters)
        {
            var query = Context.Set<StaffQueryResult>().FromSqlInterpolated($@"
select 
    o.Id OfficeId, o.OfficeName, 
    s.Id StaffId, s.StaffName, 
    o.MultiOfficeId,
    o.BrandId,
    o.StateId,
    st.CountryId,
    s.IsActive
from Staff s
join Office o on o.Id = s.OfficeId
join State st on st.Id = o.StateId
");
            
            if (parameters.BrandId.HasValue)
                query = query.Where(x => x.OfficeId == parameters.OfficeId);

            if (parameters.CountryId.HasValue)
                query = query.Where(x => x.CountryId == parameters.CountryId);

            if (parameters.StateId.HasValue)
                query = query.Where(x => x.StateId == parameters.StateId);

            if (parameters.OfficeId.HasValue)
                query = query.Where(x => x.OfficeId == parameters.OfficeId);

            if (parameters.MultiOfficeId.HasValue)
                query = query.Where(x => x.MultiOfficeId == parameters.MultiOfficeId);

            if (!string.IsNullOrEmpty(parameters.StaffName))
                query = query.Where(x => x.StaffName.Contains(parameters.StaffName));

            if (parameters.IsActive.HasValue)
                query = query.Where(s => s.IsActive == parameters.IsActive);

            return query;
        }

        protected override IOrderedQueryable<StaffQueryResult> GetDefaultOrderBy(IQueryable<StaffQueryResult> query)
            => query.OrderBy(o => o.OfficeName).ThenBy(s => s.StaffName);
    }
}

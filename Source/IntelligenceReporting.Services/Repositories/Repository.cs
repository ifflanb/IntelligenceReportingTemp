using EFCore.BulkExtensions;
using IntelligenceReporting.Databases;
using IntelligenceReporting.Entities;
using Microsoft.EntityFrameworkCore;

namespace IntelligenceReporting.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected DbContext DbContext { get; }

    protected Repository(IntelligenceReportingDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task Add(params T[] items)
    {
        await DbContext.BulkInsertAsync(items);
    }

    public Task<T[]> GetAll()
    {
        var results = DbContext.Set<T>().ToArray();
        return Task.FromResult(results);
    }

    public async Task Update(params T[] items)
    {
        await DbContext.BulkUpdateAsync(items);
    }

    public async Task Delete(params T[] items)
    {
        await DbContext.BulkDeleteAsync(items);
    }
}


public class BrandRepository : Repository<Brand> { public BrandRepository(IntelligenceReportingDbContext dbContext) : base(dbContext) { } }
public class CountryRepository : Repository<Country> { public CountryRepository(IntelligenceReportingDbContext dbContext) : base(dbContext) { } }
public class LandAreaTypeRepository : Repository<LandAreaType> { public LandAreaTypeRepository(IntelligenceReportingDbContext dbContext) : base(dbContext) { } }
public class MultiOfficeRepository : Repository<Office> { public MultiOfficeRepository(IntelligenceReportingDbContext dbContext) : base(dbContext) { } }
public class OfficeRepository : Repository<Office> { public OfficeRepository(IntelligenceReportingDbContext dbContext) : base(dbContext) { } }
public class ReportMethodOfSalesRepository : Repository<ReportMethodOfSale> { public ReportMethodOfSalesRepository(IntelligenceReportingDbContext dbContext) : base(dbContext) { } }
public class RoleRepository : Repository<Role> { public RoleRepository(IntelligenceReportingDbContext dbContext) : base(dbContext) { } }
public class SaleLifeRepository : Repository<SaleLife> { public SaleLifeRepository(IntelligenceReportingDbContext dbContext) : base(dbContext) { } }
public class SourceRepository : Repository<Source> { public SourceRepository(IntelligenceReportingDbContext dbContext) : base(dbContext) { } }
public class StaffRepository : Repository<Staff> { public StaffRepository(IntelligenceReportingDbContext dbContext) : base(dbContext) { } }
public class StateRepository : Repository<State> { public StateRepository(IntelligenceReportingDbContext dbContext) : base(dbContext) { } }
public class StatusRepository : Repository<Status> { public StatusRepository(IntelligenceReportingDbContext dbContext) : base(dbContext) { } }

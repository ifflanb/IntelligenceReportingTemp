using IntelligenceReporting.Entities;
using IntelligenceReporting.Queries;
using Microsoft.EntityFrameworkCore;

namespace IntelligenceReporting.Databases
{
    public class IntelligenceReportingDbContext : DbContext
    {
        public IntelligenceReportingDbContext(DbContextOptions<IntelligenceReportingDbContext> options) : base(options)
        {
        }

        public DbSet<ReportMethodOfSale> ReportMethodOfSale => Set<ReportMethodOfSale>();
        public DbSet<Brand> Brand => Set<Brand>();
        public DbSet<Role> Role => Set<Role>();
        public DbSet<MultiOffice> MultiOffice => Set<MultiOffice>();
        public DbSet<Country> Country => Set<Country>();
        public DbSet<LandAreaType> LandAreaType => Set<LandAreaType>();
        public DbSet<Office> Office => Set<Office>();
        public DbSet<SaleLife> SaleLife => Set<SaleLife>();
        public DbSet<Source> Source => Set<Source>();
        public DbSet<Staff> Staff => Set<Staff>();
        public DbSet<State> State => Set<State>();
        public DbSet<Status> Status => Set<Status>();

        public DbSet<AgentEarningsQueryResult> AgentEarningsQueryResult => Set<AgentEarningsQueryResult>();
        public DbSet<OfficeQueryResult> OfficeQueryResult => Set<OfficeQueryResult>();
        public DbSet<StaffQueryResult> StaffQueryResult => Set<StaffQueryResult>();
    }
}
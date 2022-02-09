using System.Diagnostics;
using EFCore.BulkExtensions;
using IntelligenceReporting.Databases;
using IntelligenceReporting.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IntelligenceReporting.Services
{
    public class VaultSyncService : IVaultSyncService
    {
        private readonly DbContext _dbContext;
        private readonly IVaultDatabase _vaultDatabase;
        private readonly IVaultDatabase2 _vaultDatabase2;
        private readonly ILogger _logger;

        public VaultSyncService(
            IntelligenceReportingDbContext dbContext, 
            IVaultDatabase vaultDatabase, 
            IVaultDatabase2 vaultDatabase2, 
            ILogger<VaultSyncService> logger)
        {
            _dbContext = dbContext;
            _vaultDatabase = vaultDatabase;
            _vaultDatabase2 = vaultDatabase2;
            _logger = logger;
        }

        public async Task<TimeSpan> Sync()
        {
            var stopwatch = Stopwatch.StartNew();
            await LogTiming("sync", async () =>
            {
                var multiOfficesHash = await LogTiming("sync multi-offices", SyncMultiOffices);
                var brandsHash = await LogTiming("sync brands", SyncBrands);
                var countriesHash = await LogTiming("sync countries", SyncCountries);
                var statesHash = await LogTiming("sync states", () => SyncStates(countriesHash));
                var officesHash = await LogTiming("sync offices", () => SyncOffices(brandsHash, statesHash, multiOfficesHash));
                var staffHash = await LogTiming("sync staff", () => SyncStaff(officesHash));
                await LogTiming("sync sale lives", () => SyncSaleLives(officesHash, staffHash));
                return "";
            });
            return stopwatch.Elapsed;
        }

        private async Task<T> LogTiming<T>(string operationDescription, Func<Task<T>> operation)
        {
            _logger.LogInformation("Starting {operationDescription}", operationDescription);
            var stopwatch = Stopwatch.StartNew();

            var result = await operation();

            var duration = stopwatch.Elapsed;
            _logger.LogInformation("Completed {operationDescription} after {duration}", operationDescription, duration);
            return result;
        }

        private async Task LogTiming(string operationDescription, Func<Task> operation)
        {
            await LogTiming(operationDescription, async () =>
            {
                await operation();
                return "";
            });
        }

        private async Task<Dictionary<int, T>> GetHash<T>(Func<IQueryable<T>, IQueryable<T>>? extendWhere = null) where T: SynchronizedEntity
        {
            var query = _dbContext.Set<T>().Where(x => x.SourceId == SourceId.VaultRe);
            if (extendWhere != null)
                query = extendWhere(query);
            var result = await query.ToDictionaryAsync(x => x.ExternalId);
            return result;
        }

        private delegate Task SyncChildren<T>(T[] entitiesFromVault, int[] externalIds, Dictionary<int, T> hash) where T : SynchronizedEntity;

        private async Task Sync<T>(T[] entitiesFromVault, SyncChildren<T> syncChildren) where T : SynchronizedEntity
        {
            if (entitiesFromVault.Length == 0) return;

            var externalIds = entitiesFromVault.Select(x => x.ExternalId).ToArray();
            var hash = await GetHash<T>(query => query.Where(x => externalIds.Contains(x.ExternalId)));

            await syncChildren(entitiesFromVault, externalIds, hash);

            await Sync(entitiesFromVault, null, hash);
        }

        private async Task<Dictionary<int, T>> Sync<T>(T[] entitiesFromVault, Func<T, T, bool> areDifferent) where T: SynchronizedEntity
        {
            if (entitiesFromVault.Length == 0) return new Dictionary<int, T>();

            var hash = await GetHash<T>();
            var changes = await Sync(entitiesFromVault, areDifferent, hash);
            if (changes == 0) return hash;

            return await GetHash<T>();
        }

        private async Task<int> Sync<T>(T[] entitiesFromVault, Func<T, T, bool>? areDifferent, Dictionary<int, T> hash) where T : SynchronizedEntity
        {
            // Find entities to insert and to update
            var toInsert = new List<T>();
            var toUpdate = new List<T>();
            foreach (var fromVault in entitiesFromVault)
            {
                if (hash.TryGetValue(fromVault.ExternalId, out var existing))
                {
                    if (areDifferent == null || areDifferent(fromVault, existing))
                    {
                        fromVault.Id = existing.Id;
                        toUpdate.Add(fromVault);
                    }
                }
                else
                {
                    toInsert.Add(fromVault);
                }
            }

            var changes = toInsert.Count + toUpdate.Count;
            if (changes == 0) return 0;

            // Ensure they're all sourced from Vault.
            foreach (var e in entitiesFromVault)
            {
                e.SourceId = SourceId.VaultRe;
            }

            // Insert new entities and update existing ones
            _logger.LogInformation(" - {typeName}: Inserting {insertCount}, Updating {updateCount}.", typeof(T).Name, toInsert.Count, toUpdate.Count);
            await _dbContext.BulkInsertAsync(toInsert, opts => opts.IncludeGraph = true);
            await _dbContext.BulkUpdateAsync(toUpdate, opts => opts.IncludeGraph = true);

            return changes;
        }

        private async Task<Dictionary<int,Brand>> SyncBrands()
        {
            var sql = @"
select af.franchiseid, af.franchisename from ACCOUNT_FRANCHISE af 
";
            var fromVault = await _vaultDatabase.Query(sql, r =>
            {
                var entity = new Brand();
                var field = 0;
                entity.ExternalId = r.GetInt32(field++);
                entity.BrandName = r.GetString(field);
                return entity;
            });

            return await Sync(fromVault, (a, b) => a.BrandName != b.BrandName);
        }

        private async Task<Dictionary<int, Country>> SyncCountries()
        {
            var sql = @"
select countryid, countryname, isocountrycode from COUNTRY c
";
            var fromVault = await _vaultDatabase.Query(sql, r =>
            {
                var field = 0;
                return new Country
                {
                    ExternalId = r.GetInt32(field++),
                    CountryName = r.GetString(field++),
                    IsoCountryCode = r.GetNullableString(field) ?? "",
                };
            });

            return await Sync(fromVault, (a, b) => a.CountryName != b.CountryName || a.IsoCountryCode != b.IsoCountryCode);
        }

        private async Task<Dictionary<int, MultiOffice>> SyncMultiOffices()
        {
            var sql = @"
select id, groupname from MULTIOFFICE_GROUPING
";
            var fromVault = await _vaultDatabase.Query(sql, r =>
            {
                var entity = new MultiOffice();
                var field = 0;
                entity.ExternalId = r.GetInt32(field++);
                entity.MultiOfficeName = r.GetString(field);
                return entity;
            });

            return await Sync(fromVault, (a, b) => a.MultiOfficeName != b.MultiOfficeName);
        }

        private async Task<Dictionary<int, State>> SyncStates(Dictionary<int, Country> countriesHash)
        {
            var sql = @"
select s.stateid, s.statename, s.abbrev, s.pcountryid from STATELIST s
";
            var fromVault = await _vaultDatabase.Query(sql, r =>
            {
                var entity = new State();
                var field = 0;
                entity.ExternalId = r.GetInt32(field++);
                entity.StateName = r.GetNullableString(field++) ??"";
                entity.StateAbbreviation = r.GetNullableString(field++) ??"";
                var countryIdFromVault = r.GetInt32(field);
                entity.CountryId = countriesHash[countryIdFromVault].Id;
                entity.TimeZoneName = GetTimeZoneName(entity.StateName);
                return entity;
            });

            return await Sync(fromVault, (a, b) => a.StateName != b.StateName || a.StateAbbreviation != b.StateAbbreviation);
        }

        private async Task<Dictionary<int, Office>> SyncOffices(
            Dictionary<int, Brand> brandsHash,
            Dictionary<int, State> statesHash, 
            Dictionary<int, MultiOffice> multiOfficesHash)
        {
            var sql = @"
select a.accountid, a.fullname, a.franchiseid, a.stateid, a.modifydate,
    (select max(ma.id) from MULTIOFFICE_ACCOUNT ma where ma.accountid = a.accountid),
    if(a.demoaccount = 0 AND a.locked = 0, 1, 0) IsActive
from ACCOUNT a
";
            var fromVault = await _vaultDatabase.Query(sql, row =>
            {
                var entity = new Office();
                var field = 0;
                entity.ExternalId = row.GetInt32(field++);
                entity.OfficeName = row.GetString(field++);
                var brandId = row.GetNullableInt32(field++);
                var stateId = row.GetNullableInt32(field++);
                entity.LastUpdated = row.GetDateTime(field++);
                var multiOfficeId = row.GetNullableInt32(field++);
                entity.IsActive = row.GetBoolean(field);

                entity.BrandId = brandId == null ? null : brandsHash[brandId.Value].Id;
                entity.StateId = stateId == null ? null : statesHash[stateId.Value].Id;
                entity.MultiOfficeId = multiOfficeId == null ? null : multiOfficesHash[multiOfficeId.Value].Id;

                return entity;
            });

            return await Sync(fromVault, (a, b) => a.LastUpdated != b.LastUpdated);
        }

        private async Task<Dictionary<int, Staff>> SyncStaff(Dictionary<int, Office> officesHash)
        {
            var sql = @"
select au.accountid, au.userid, au.firstname, au.lastname, if(au.deleted = 0 and au.islogin = 1, 1, 0) IsActive,
	au.lastmodified
from ACCOUNTUSER au
where au.accountid is not null
";
            var fromVault = await _vaultDatabase.Query(sql, row =>
            {
                var entity = new Staff();
                var field = 0;
                var accountId = row.GetInt32(field++);
                entity.OfficeId = officesHash[accountId].Id;
                entity.ExternalId = row.GetInt32(field++);
                var firstName = row.GetNullableString(field++);
                var lastName = row.GetNullableString(field++);
                entity.IsActive = row.GetBoolean(field++);
                entity.LastUpdated = row.GetNullableDateTime(field) ?? DatabaseExtensions.DateZero;

                entity.StaffName = $"{firstName} {lastName}".Trim();
                if (entity.StaffName.Length > 100)
                    entity.StaffName = entity.StaffName[..100];

                return entity;
            });

            // ToDo: if the office changes, we need to update the SaleLifeAgent and SaleLifeEarnings office ids
            return await Sync(fromVault, (a, b) => a.LastUpdated != b.LastUpdated);
        }


        private async Task SyncSaleLives(Dictionary<int, Office> officesHash, Dictionary<int, Staff> staffHash)
        {
            const int batchSize = 10000;
            DateTimeOffset lastUpdated;
            var any = await _dbContext.Set<SaleLife>().AnyAsync();
            if (any)
                lastUpdated = await _dbContext.Set<SaleLife>().MaxAsync(x => x.LastUpdated);
            else
                lastUpdated = DatabaseExtensions.DateZero;

            var sql = @$"
select distinct
    p.accountid, 
    p.propertyid, 
    concat(coalesce(p.unit, ''), if(p.unit <> '', '/', ''), p.streetnum, ' ', p.street) address,
    pc.classid,
    pc.classname,
    pls.lifeid, 
    pls.moddate, 
    pls.methodid, 
    pls.authoritytypeid,
    pls.currentstatus, 
    pls.is_unit_title,
    pls.appraisaldate, 
    pls.authoritystart, 
    pls.auctiondatetime,
    pls.conditional, 
    pls.unconditional, 
    pls.settlement, 
    pls.settlementactioned, 
    pls.admincomplete, 
    pls.admincompleteactioned,
    pls.grosscommission, 
    pls.grosscommissionlessdeductions,
    (select sum(pd.amount) from PREDISTRIBUTION_DEDUCTIONS pd where pd.salelifeid = pls.lifeid and pd.feeid = -3) FoundationContribution,
    pls.publishagentid1,
    pls.publishagentid2,
    pls.commercialListingType,
    p.landareatype, 
    p.landarea
from PROPERTY p
join PROPERTYLIFE_HISTORY ph on ph.propertyid  = p.propertyid 
join PROPERTYLIFE_SALE pls on pls.lifeid = ph.salelifeid
join PROPERTYSTATUS ps on ps.statusid = pls.currentstatus 
join PROPERTYCLASS pc on pc.classid = p.propertyclass 
where pls.currentstatus not in (1, 10)
and pls.moddate >= '{lastUpdated:yyyy-MM-dd HH:mm:ss}'
and p.currentlifesale is not null # required to work around pls.lifeid 347856 having two ph's
order by pls.moddate
";

            var totalCount = 0;
            var fromVault = new List<SaleLife>();
            await _vaultDatabase.Query(sql, async row =>
            {
                var entity = new SaleLife();
                var field = 0;
                var accountId = row.GetInt32(field++);
                entity.OfficeId = officesHash[accountId].Id;
                var propertyId = row.GetInt32(field++);
                entity.Address = row.GetNullableString(field++) + "";
                var propertyClassId = row.GetInt32(field++);
                var propertyClassName = row.GetString(field++);
                entity.ExternalId = row.GetInt32(field++);
                entity.LastUpdated = row.GetDateTime(field++);
                var methodOfSaleId = row.GetNullableInt32(field++) ?? 1; // property 157 has null
                var authorityTypeId = row.GetNullableInt32(field++) ?? 0;
                entity.ReportMethodOfSaleId = GetReportMethodOfSale(methodOfSaleId, authorityTypeId);
                var status = row.GetInt32(field++);
                entity.StatusId = GetStatusId(status);
                entity.IsUnitTitle = row.GetBoolean(field++);
                entity.AppraisalDate = row.GetNullableDate(field++);
                entity.ListingDate = row.GetNullableDate(field++);
                entity.AuctionDate = row.GetNullableDate(field++);
                entity.ConditionalDate = row.GetNullableDate(field++);
                entity.UnconditionalDate = row.GetNullableDate(field++);
                entity.SettlementDate = row.GetNullableDate(field++);
                entity.SettlementActioned = row.GetNullableDateTime(field++);
                entity.AdminComplete = row.GetNullableDate(field++);
                entity.AdminCompleteActioned = row.GetNullableDateTime(field++);
                entity.GrossCommission = row.GetNullableDecimal(field++);
                entity.OfficeGrossIncome = row.GetNullableDecimal(field++);
                entity.FoundationContribution = row.GetNullableDecimal(field++);                
                var agent1 = row.GetNullableInt32(field++);
                var agent2 = row.GetNullableInt32(field++);
                var commercialListingType = row.GetString(field++);                
                entity.ListingTypeId = GetListingType(commercialListingType);
                entity.LandAreaTypeId = GetLandAreaType(row.GetNullableInt32(field++));
                entity.LandArea = row.GetNullableDecimal(field);
                entity.PropertyClassId = GetPropertyClass(propertyClassId);

                void AddSaleLifeAgent(int? agentId, bool isPrimary = false)
                {
                    if (agentId == null || agentId < 0) return;
                    var staff = staffHash[agentId.Value];
                    entity.Agents.Add(new SaleLifeAgent { StaffId = staff.Id, OfficeId = staff.OfficeId, IsPrimary = isPrimary });
                }
                AddSaleLifeAgent(agent1, true);
                AddSaleLifeAgent(agent2);

                fromVault.Add(entity);

                if (fromVault.Count == batchSize) await CommitResults();
            });
            await CommitResults();


            async Task CommitResults()
            {
                // Unfortunately, some sale lives have multiple histories, just pick one.
                fromVault = fromVault.GroupBy(x => x.ExternalId).Select(g => g.Last()).ToList();

                totalCount += fromVault.Count;
                _logger.LogInformation("Committing {count} sale lives - total = {totalCount}", fromVault.Count, totalCount);

                await Sync(fromVault.ToArray(), SyncSaleLifeChildren);
                fromVault.Clear();

                _logger.LogInformation("Committed {count} sale lives - total = {totalCount}", fromVault.Count, totalCount);
            }


            async Task SyncSaleLifeChildren(SaleLife[] saleLivesFromVault, int[] externalIds, Dictionary<int, SaleLife> ourHash)
            {
                var vaultHash = saleLivesFromVault.ToDictionary(x => x.ExternalId);
                var sql = @$"
SELECT cs.salelifeid, cs.splittypeid, cs.userid, cs.percentsplit, cs.grosssplit, cs.netsplit
FROM COMMISSIONSPLIT cs
WHERE cs.salelifeid IN ({string.Join(", ", externalIds)})
AND cs.splittypeid IN (1, 2, 3, 7, 8)
AND cs.userid > 0
";
                await _vaultDatabase2.Query(sql, row =>
                {
                    var result = new SaleLifeEarnings();
                    var field = 0;
                    var saleLifeId = row.GetInt32(field++);
                    var splitTypeId = row.GetInt32(field++);
                    var userid = row.GetInt32(field++);
                    result.SplitPercent = (row.GetNullableDecimal(field++) ?? 0) / 100;
                    result.GrossSplit = row.GetNullableDecimal(field++) ?? 0;
                    result.NetSplit = row.GetNullableDecimal(field) ?? 0;

                    var saleLife = vaultHash[saleLifeId];
                    var staff = staffHash[userid];
                    result.StaffId = staff.Id;
                    result.OfficeId = staff.OfficeId;

                    switch (splitTypeId)
                    {
                        case 1: // Lister
                            result.RoleId = RoleId.Lister;
                            break;

                        case 2: // Seller
                            result.RoleId = RoleId.Seller;
                            break;

                        case 3: // Lister + seller
                            result.RoleId = RoleId.Seller;
                            result.GrossSplit /= 2;
                            result.NetSplit /= 2;
                            result.SplitPercent /= 2;

                            saleLife.Earnings.Add(new SaleLifeEarnings
                            {
                                SplitPercent = result.SplitPercent,
                                GrossSplit = result.GrossSplit,
                                NetSplit = result.NetSplit,
                                OfficeId = result.OfficeId,
                                StaffId = result.StaffId,
                                RoleId = RoleId.Lister,
                            });
                            break;

                        case 7: // Referrer
                            result.RoleId = RoleId.Referrer;
                            break;

                        case 8: // Other
                            result.RoleId = RoleId.Other;
                            break;

                        default: 
                            throw new InvalidOperationException("We shouldn't be getting splitTypeid = " + splitTypeId);
                    }

                    saleLife.Earnings.Add(result);
                });
            }

            _logger.LogInformation("{totalCount} sale lives updated", totalCount);
        }

        private static LandAreaTypeId? GetLandAreaType(int? landAreaType) =>
            landAreaType switch
            {
                1 => LandAreaTypeId.SquareMeters,
                2 => LandAreaTypeId.Acre,
                3 => LandAreaTypeId.Hectare,
                null => null,
                _ => throw new InvalidOperationException("Unexpected VaultRE LandAreaType = " + landAreaType)
            };

        private static ListingTypeId GetListingType(string listingType) => 
            listingType.ToLower() switch
        {
            "sale" => ListingTypeId.Sale,
            "lease" => ListingTypeId.Lease,
            "both" => ListingTypeId.Both,
            _ => ListingTypeId.Sale,
        };

        private static PropertyClassId GetPropertyClass(int propertyClassId) =>
            propertyClassId switch
        {
            1 => PropertyClassId.Residential,
            2 => PropertyClassId.Commercial,
            3 => PropertyClassId.Business,
            4 => PropertyClassId.Rural,
            5 => PropertyClassId.HolidayRental,
            6 => PropertyClassId.Land,
            7 => PropertyClassId.Livestock,
            8 => PropertyClassId.ClearingSales,                
            _ => throw new InvalidOperationException("Unexpected VaultRE PropertyClass = " + propertyClassId)
        };        

        private static ReportMethodOfSaleId GetReportMethodOfSale(int methodOfSaleId, int authorityTypeId)
        {
            if (methodOfSaleId == 2) return ReportMethodOfSaleId.Auction;
            if (methodOfSaleId == 3) return ReportMethodOfSaleId.Tender;
            if (new[] { 1, 5, 8, 10, 11 }.Contains(authorityTypeId)) return ReportMethodOfSaleId.Exclusive;
            return ReportMethodOfSaleId.General;
        }

        private static StatusId GetStatusId(int statusId) =>
            statusId switch
            {
                1 => throw new NotSupportedException("Prospect status is not supported"),
                2 => StatusId.Appraisal,
                3 => StatusId.Listing,
                4 => StatusId.Conditional,
                5 => StatusId.Unconditional,
                6 => StatusId.Settled,
                7 => StatusId.ManagementListing,
                8 => StatusId.WithdrawnListing,
                9 => StatusId.WithdrawnAppraisal,
                10 => StatusId.FallenSale,
                _ => throw new InvalidOperationException("Unexpected VaultRE statusid: " + statusId)
            };

        private static string GetTimeZoneName(string stateName) =>
            stateName switch
            {
                "Western Australia" => "W. Australia Standard Time",
                "New South Wales" => "AUS Eastern Standard Time",
                "South Australia" => "Cen. Australia Standard Time",
                "Queensland" => "E. Australia Standard Time",
                "Australian Capital Territory" => "AUS Eastern Standard Time",
                "Northern Territory" => "AUS Central Standard Time",
                "Tasmania" => "AUS Eastern Standard Time",
                "Victoria" => "AUS Eastern Standard Time",
                "United Kingdom" => "GMT Standard Time",
                "Indonesia" => "SE Asia Standard Time",
                "Singapore" => "Singapore Standard Time",
                "New Zealand" => "New Zealand Standard Time",
                "Fiji" => "Fiji Standard Time",
                _ => throw new InvalidOperationException("Unexpected VaultRE statename: " + stateName)

            };
    }
}

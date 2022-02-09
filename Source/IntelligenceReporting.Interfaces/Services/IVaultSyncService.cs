namespace IntelligenceReporting.Services;

public interface IVaultSyncService
{
    Task<TimeSpan> Sync();
}
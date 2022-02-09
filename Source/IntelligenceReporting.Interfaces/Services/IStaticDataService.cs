using IntelligenceReporting.Entities;

namespace IntelligenceReporting.Services;

public interface IStaticDataService
{
    Task<StaticData> GetStaticData();
}
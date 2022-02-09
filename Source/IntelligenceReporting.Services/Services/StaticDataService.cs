using IntelligenceReporting.Entities;
using IntelligenceReporting.Repositories;

namespace IntelligenceReporting.Services
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly IRepository<Source> _sourceRepository;
        private readonly IRepository<Status> _statusRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<State> _stateRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<ReportMethodOfSale> _reportMethodOfSaleRepository;
        private readonly IRepository<LandAreaType> _landAreaTypeRepository;

        public StaticDataService(
            IRepository<Brand> brandRepository, 
            IRepository<Source> sourceRepository,
            IRepository<Status> statusRepository, 
            IRepository<Role> roleRepository,
            IRepository<State> stateRepository, 
            IRepository<Country> countryRepository, 
            IRepository<ReportMethodOfSale> reportMethodOfSaleRepository, 
            IRepository<LandAreaType> landAreaTypeRepository)
        {
            _brandRepository = brandRepository;
            _sourceRepository = sourceRepository;
            _statusRepository = statusRepository;
            _roleRepository = roleRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
            _reportMethodOfSaleRepository = reportMethodOfSaleRepository;
            _landAreaTypeRepository = landAreaTypeRepository;
        }

        public async Task<StaticData> GetStaticData()
        {
            var result = new StaticData
            {
                Sources = await _sourceRepository.GetAll(),
                Brands = await _brandRepository.GetAll(),
                Countries = await _countryRepository.GetAll(),
                LandAreaTypes = await _landAreaTypeRepository.GetAll(),
                Statuses = await _statusRepository.GetAll(),
                States = await _stateRepository.GetAll(),
                Roles = await _roleRepository.GetAll(),
                ReportMethodOfSales = await _reportMethodOfSaleRepository.GetAll()
            };
            return result;
        }
    }
}

using VeloSpace.DTOs.SatelliteDTOS;
using VeloSpace.Repositories.SatellitesRepositories;
using VeloSpace.Services.RocketServices;

namespace VeloSpace.Services.SatellitesServices;

public class SatelliteService : ISatelliteService
{
    private readonly ISatelliteRepository _satelliteRepository;
    private readonly IRocketService _rocketService;

    public SatelliteService(ISatelliteRepository satelliteRepository, IRocketService rocketService)
    {
        _satelliteRepository = satelliteRepository;
        _rocketService = rocketService;
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message){} 
    }
    
    public async Task<SatelliteDTO> GetByIdAsync(long id)
    {
        var search = await _satelliteRepository.GetByIdAsync(id);

        if (search == null) throw new NotFoundException($"Satellite with id {id} not found");

        return new SatelliteDTO
        {
            Height = search.Height,
            LaunchProviderId = search.LaunchProviderId,
            LaunchJustification = search.LaunchJustification,
            Length = search.Length,
            Name = search.Name,
            RocketId = search.RocketId,
            SatelliteId = search.SatelliteId,
            SatellitePriorityId = search.SatellitePriorityId,
            SatelliteStatusId = search.SatelliteStatusId,
            ShipperId = search.ShipperId,
            Width = search.Width,
            Weight = search.Weight,
            TrackingCode = search.TrackingCode
        };
    }

    public async Task<SatellitePriorityDTO> GetPriorityByIdAsync(long id)
    {
        var search = await _satelliteRepository.GetPriorityByIdAsync(id);
        
        if (search == null) throw new NotFoundException($"Satellite Status with id {id} not found");

        return new SatellitePriorityDTO
        {
            SatellitePriorityId = search.SatellitePriorityId,
            Description = search.Description,
            Level = search.Level
        };
    }

    public async Task<List<long>> SatelliteAllocation(long rocketId, List<long> satellitesIdList)
    {
        // função de calcular
        // transformar peso e volume numa coisa só : densidade
        // volume = width x length
        // densidade = P/V
        // D = P/V = xKg/m^2

        // receber o id do foguete
        var rocket = await _rocketService.GetByIdAsync(rocketId);

        // calcular capacidade do foguete como densidade
        var baseAreaRocket = rocket.CapacityWidth * rocket.CapacityLength;

        var volumeRocket = baseAreaRocket * rocket.CapacityHeight;

        var rocketCapacityDensity = rocket.CapacityWeight / volumeRocket;


        // receber uma lista de ids de satellites
        // dar get em cada um
        // adicionar cada um num dicionario?
        // chave é o Id do satellite
        // valor é uma lista com densidade e prioridade

        Dictionary<long, List<int>> satDensityPriority = new Dictionary<long, List<int>>();

        foreach (var sat in satellitesIdList)
        {
            var satellite = await GetByIdAsync(sat);

            var satellitePriority = await GetPriorityByIdAsync(satellite.SatelliteStatusId);

            var baseAreaSatellite = satellite.Width * satellite.Length;

            var volumeSatellite = baseAreaSatellite * satellite.Height;

            var satelliteDensity = satellite.Weight / volumeSatellite;
            
            satDensityPriority.Add(sat, new List<int>{satelliteDensity, satellitePriority.Level});
        }
        
        // calcular primeiro os satellites de maior prioridade
        // calcular densidade de cada um 
        // variável 1 = densidade
        // variável 2 = prioridade
        // problema da mochila

    }
}
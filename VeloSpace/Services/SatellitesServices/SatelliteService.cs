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

    public async Task<List<long>> SatelliteAllocation(long rocketId, List<long> satellitesIdList)
    {
        // função de calcular
        // transformar peso e volume numa coisa só : densidade
        // densidade = P/V
        // D = P/V = xKg/m^2

        // receber o id do foguete
        var rocket = await _rocketService.GetByIdAsync(rocketId);

        // calcular capacidade do foguete como densidade
        var volumeRocket = rocket.CapacityHeight + rocket.CapacityLength + rocket.CapacityWidth;
        
        var capacityDensityRocket = volumeRocket asdasdasd

        throw new NotImplementedException();

        // salvar isso numa variável?


        // receber uma lista de ids de satellites
        // dar get em cada um
        // calcular primeiro os satellites de maior prioridade
        // calcular densidade de cada um 
        // variável 1 = densidade
        // variável 2 = prioridade
        // problema da mochila

    }
}
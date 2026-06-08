using VeloSpace.DTOs.SatelliteDTOS;
using VeloSpace.Repositories.SatellitesRepositories;

namespace VeloSpace.Services.SatellitesServices;

public class SatelliteService : ISatelliteService
{
    private readonly ISatelliteRepository _satelliteRepository;

    public SatelliteService(ISatelliteRepository satelliteRepository)
    {
        _satelliteRepository = satelliteRepository;
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
}
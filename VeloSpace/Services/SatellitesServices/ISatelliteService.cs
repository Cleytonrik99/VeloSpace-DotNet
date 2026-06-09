using VeloSpace.DTOs.SatelliteDTOS;

namespace VeloSpace.Services.SatellitesServices;

public interface ISatelliteService
{
    Task<SatelliteDTO> GetByIdAsync(long id);

    Task<SatellitePriorityDTO> GetPriorityByIdAsync(long id);

    Task<List<long>> SatelliteAllocation(long rocketId, List<long> satellitesIdList);
}
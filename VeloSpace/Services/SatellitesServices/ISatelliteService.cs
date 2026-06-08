using VeloSpace.DTOs.SatelliteDTOS;

namespace VeloSpace.Services.SatellitesServices;

public interface ISatelliteService
{
    Task<SatelliteDTO> GetByIdAsync(long id);
}
using VeloSpace.Model.SatelliteShi;

namespace VeloSpace.Repositories.SatellitesRepositories;

public interface ISatelliteRepository
{
    Task<Satellite> GetByIdAsync(long id);
}
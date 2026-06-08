using VeloSpace.Context;
using VeloSpace.Model.SatelliteShi;

namespace VeloSpace.Repositories.SatellitesRepositories;

public class SatelliteRepository : ISatelliteRepository
{
    private readonly VeloSpaceContext _context;

    public SatelliteRepository(VeloSpaceContext context)
    {
        _context = context;
    }

    public async Task<Satellite> GetByIdAsync(long id)
    {
        return await _context.Satellite.FindAsync(id);
    }
}
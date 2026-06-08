using Microsoft.EntityFrameworkCore;
using VeloSpace.Context;
using VeloSpace.Model.RocketShi;

namespace VeloSpace.Repositories.RocketRepositories;

public class RocketRepository : IRocketRepository
{
    private readonly VeloSpaceContext _context;
    
    public RocketRepository(VeloSpaceContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Rocket>> GetAllAsync()
    {
        return await _context.Rocket.ToListAsync();
    }

    public async Task<Rocket> GetByIdAsync(long id)
    {
        return await _context.Rocket.FindAsync(id);
    }

    public async Task AddAsync(Rocket rocket)
    {
        _context.Rocket.Add(rocket);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Rocket rocket)
    {
        _context.Rocket.Update(rocket);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var search = await GetByIdAsync(id);
        _context.Remove(search);
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<Rocket> Items, int TotalItems)> SearchAsync(string? name, int? capacityHeight, int? capacityWidth, int? capacityLength, int? capacityWeight, long? rocketStatusId, int page, int pageSize, string sortBy, string sortDir)
    {
        var query = _context.Rocket.AsQueryable();
        
        if (!string.IsNullOrEmpty(name)) query = query.Where(r => r.Name == name);

        if (capacityHeight.HasValue) query = query.Where(r => r.CapacityHeight == capacityHeight);
        if (capacityWidth.HasValue) query = query.Where(r => r.CapacityWidth == capacityWidth);
        if (capacityLength.HasValue) query = query.Where(r => r.CapacityLength == capacityLength);
        if (capacityWeight.HasValue) query = query.Where(r => r.CapacityWeight == capacityWeight);
        
        if (rocketStatusId.HasValue) query = query.Where(r => r.RocketStatusId == rocketStatusId);
        
        var totalItems = await query.CountAsync();
        
        bool desc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);

        query = sortBy?.ToLowerInvariant() switch
        {
            "name" => desc ? query.OrderByDescending(r => r.Name) : query.OrderBy(r => r.Name),
            "capacityHeight" => desc ? query.OrderByDescending(r => r.CapacityHeight) : query.OrderBy(r => r.CapacityHeight),
            "capacityWidth" => desc ? query.OrderByDescending(r => r.CapacityWidth) : query.OrderBy(r => r.CapacityWidth),
            "capacityLength" => desc ? query.OrderByDescending(r => r.CapacityLength) : query.OrderBy(r => r.CapacityLength),
            "capacityWeight" => desc ? query.OrderByDescending(r => r.CapacityWeight) : query.OrderBy(r => r.CapacityWeight),
            "rocketStatusId" => desc ? query.OrderByDescending(r => r.RocketStatusId) : query.OrderBy(r => r.RocketStatusId),
            _ => desc ? query.OrderByDescending(r => r.RocketId) : query.OrderBy(r => r.RocketId)
        };
        
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;
        
        var skip = (page - 1) * pageSize;

        var data = await query.Skip(skip).Take(pageSize).ToListAsync();

        return (data, totalItems);
    }
}
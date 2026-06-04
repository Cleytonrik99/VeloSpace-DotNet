using Microsoft.EntityFrameworkCore;
using VeloSpace.Context;
using VeloSpace.DTOs.Shippers;
using VeloSpace.Model.ShipperShi;

namespace VeloSpace.Repositories.ShippersRepositories;

public class ShipperRepository : IShipperRepository
{
    private readonly VeloSpaceContext _context;

    public ShipperRepository(VeloSpaceContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Shipper>> GetAllAsync()
    {
        return await _context.Shipper.ToListAsync();
    }

    public async Task<Shipper> GetByIdAsync(long id)
    {
        return await _context.Shipper.FindAsync(id);
    }

    public async Task AddAsync(Shipper shipper)
    {
        _context.Shipper.Add(shipper);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Shipper shipper)
    {
        _context.Shipper.Update(shipper);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var search = await GetByIdAsync(id);
        _context.Remove(search);
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<Shipper> Items, int TotalItems)> SearchAsync(string? name, string? type, int page, int pageSize, string sortBy, string sortDir)
    {
        var query = _context.Shipper.AsQueryable();

        if (!string.IsNullOrEmpty(name)) query = query.Where(s => s.Name == name);

        if (!string.IsNullOrEmpty(type)) query = query.Where(s => s.Type == type);

        var totalItems = await query.CountAsync();
        
        bool desc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);

        query = sortBy?.ToLowerInvariant() switch
        {
            "name" => desc ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name),
            "type" => desc ? query.OrderByDescending(s => s.Type) : query.OrderBy(s => s.Type),
            _ => desc ? query.OrderByDescending(s => s.ShipperId) : query.OrderBy(s => s.ShipperId)
        };
        
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;
        
        var skip = (page - 1) * pageSize;

        var data = await query.Skip(skip).Take(pageSize).ToListAsync();

        return (data, totalItems);
    }
}
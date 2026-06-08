using Microsoft.EntityFrameworkCore;
using VeloSpace.Context;
using VeloSpace.Model.Launch;

namespace VeloSpace.Repositories.LaunchProvidersRepositories;

public class LaunchProvidersRepository : ILaunchProvidersRepository
{
    private readonly VeloSpaceContext _context;

    public LaunchProvidersRepository(VeloSpaceContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LaunchProvider>> GetAllAsync()
    {
        return await _context.LaunchProvider.ToListAsync();
    }

    public async Task<LaunchProvider> GetByIdAsync(long id)
    {
        return await _context.LaunchProvider.FindAsync(id);
    }

    public async Task AddAsync(LaunchProvider launchProvider)
    {
        _context.LaunchProvider.Add(launchProvider);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(LaunchProvider launchProvider)
    {
        _context.LaunchProvider.Update(launchProvider);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var search = await GetByIdAsync(id);
        _context.Remove(search);
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<LaunchProvider> Items, int TotalItems)> SearchAsync(string? corporateName, string? cnpj, int page, int pageSize, string sortBy, string sortDir)
    {
        var query = _context.LaunchProvider.AsQueryable();

        if (!string.IsNullOrEmpty(corporateName)) query = query.Where(l => l.CorporateName == corporateName);

        if (!string.IsNullOrEmpty(cnpj)) query = query.Where(l => l.Cnpj == cnpj);
        
        var totalItems = await query.CountAsync();
        
        bool desc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);

        query = sortBy?.ToLowerInvariant() switch
        {
            "corporateName" => desc ? query.OrderByDescending(l => l.CorporateName) : query.OrderBy(l => l.CorporateName),
            "cnpj" => desc ? query.OrderByDescending(l => l.Cnpj) : query.OrderBy(l => l.Cnpj),
            _ => desc ? query.OrderByDescending(l => l.LaunchProviderId) : query.OrderBy(l => l.LaunchProviderId)
        };
        
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;
        
        var skip = (page - 1) * pageSize;

        var data = await query.Skip(skip).Take(pageSize).ToListAsync();

        return (data, totalItems);
    }
}
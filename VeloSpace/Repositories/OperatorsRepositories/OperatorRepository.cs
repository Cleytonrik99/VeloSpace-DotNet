using Microsoft.EntityFrameworkCore;
using VeloSpace.Context;
using VeloSpace.Model.OperatorShi;

namespace VeloSpace.Repositories.OperatorsRepositories;

public class OperatorRepository : IOperatorRepository
{
    private readonly VeloSpaceContext _context;

    public OperatorRepository(VeloSpaceContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Operator>> GetAllAsync()
    {
        return await _context.Operator.ToListAsync();
    }

    public async Task<Operator> GetByIdAsync(long id)
    {
        return await _context.Operator.FindAsync(id);
    }

    public async Task AddAsync(Operator @operator)
    {
        _context.Operator.Add(@operator);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Operator @operator)
    {
        _context.Operator.Update(@operator);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var search = await GetByIdAsync(id);
        _context.Remove(search);
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<Operator> Items, int TotalItems)> SearchAsync(string? name, int? cpf, long? operatorStatusId, long? launchProviderId, int page, int pageSize, string sortBy, string sortDir)
    {
        var query = _context.Operator.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name)) query = query.Where(o => o.Name == name);

        if (cpf.HasValue) query = query.Where(o => o.Cpf == cpf);

        if (operatorStatusId.HasValue) query = query.Where(o => o.OperatorStatusId == operatorStatusId);

        if (launchProviderId.HasValue) query = query.Where(o => o.LaunchProviderId == launchProviderId);
        
        var totalItems = await query.CountAsync();
        
        bool desc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);

        query = sortBy?.ToLowerInvariant() switch
        {
            "name" => desc ? query.OrderByDescending(o => o.Name) : query.OrderBy(o => o.Name),
            "cpf" => desc ? query.OrderByDescending(o => o.Cpf) : query.OrderBy(o => o.Cpf),
            "operatorStatusId" => desc ? query.OrderByDescending(o => o.OperatorStatusId) : query.OrderBy(o => o.OperatorStatusId),
            "launchProviderId" => desc ? query.OrderByDescending(o => o.LaunchProviderId) : query.OrderBy(o => o.LaunchProviderId),
            _ => desc ? query.OrderByDescending(o => o.OperatorId) : query.OrderBy(o => o.OperatorId)
        };
        
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;
        
        var skip = (page - 1) * pageSize;

        var data = await query.Skip(skip).Take(pageSize).ToListAsync();

        return (data, totalItems);
    }
}
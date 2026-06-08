using Microsoft.EntityFrameworkCore;
using VeloSpace.Context;
using VeloSpace.Model.User;

namespace VeloSpace.Repositories.UsersRepositories;

public class UserAccountRepository : IUserAccountRepository
{
    private readonly VeloSpaceContext _context;

    public UserAccountRepository(VeloSpaceContext context)
    {
        _context = context;
    }

    public async Task<UserAccount> GetByEmailAsync(string email)
    {
        return await _context.UserAccount.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<UserAccount>> GetAllAsync()
    {
        return await _context.UserAccount.ToListAsync();
    }

    public async Task<UserAccount> GetByIdAsync(long id)
    {
        return await _context.UserAccount.FindAsync(id);
    }

    public async Task AddAsync(UserAccount userAccount)
    {
        _context.UserAccount.Add(userAccount);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserAccount userAccount)
    {
        _context.UserAccount.Update(userAccount);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var search = await GetByIdAsync(id);
        _context.UserAccount.Remove(search);
        await _context.SaveChangesAsync();
    }
}
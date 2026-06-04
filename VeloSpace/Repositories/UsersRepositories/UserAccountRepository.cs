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

    public Task<IEnumerable<UserAccount>> GetAllAsync()
    {
        
    }

    public Task<UserAccount> GetByIdAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(UserAccount userAccount)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(UserAccount userAccount)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }
}
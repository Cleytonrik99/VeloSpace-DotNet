using VeloSpace.Model.User;

namespace VeloSpace.Repositories.UsersRepositories;

public interface IUserAccountRepository
{
    Task<UserAccount> GetByEmailAsync(string email);
    Task<IEnumerable<UserAccount>> GetAllAsync();

    Task<UserAccount> GetByIdAsync(long id);

    Task AddAsync(UserAccount userAccount);

    Task UpdateAsync(UserAccount userAccount);

    Task DeleteAsync(long id);
}
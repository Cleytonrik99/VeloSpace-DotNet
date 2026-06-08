using VeloSpace.Model.Launch;

namespace VeloSpace.Repositories.LaunchProvidersRepositories;

public interface ILaunchProvidersRepository
{
    Task<IEnumerable<LaunchProvider>> GetAllAsync();

    Task<LaunchProvider> GetByIdAsync(long id);

    Task AddAsync(LaunchProvider launchProvider);

    Task UpdateAsync(LaunchProvider launchProvider);

    Task DeleteAsync(long id);

    Task<(IEnumerable<LaunchProvider> Items, int TotalItems)> SearchAsync(
        string? corporateName,
        string? cnpj,
        int page,
        int pageSize,
        string sortBy,
        string sortDir
    );
}
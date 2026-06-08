using VeloSpace.Model.RocketShi;

namespace VeloSpace.Repositories.RocketRepositories;

public interface IRocketRepository
{
    Task<IEnumerable<Rocket>> GetAllAsync();
    Task<Rocket> GetByIdAsync(long id);
    Task AddAsync(Rocket rocket);
    Task UpdateAsync(Rocket rocket);
    Task DeleteAsync(long id);
    Task<(IEnumerable<Rocket> Items, int TotalItems)> SearchAsync(
        string? name,
        int? capacityHeight,
        int? capacityWidth,
        int? capacityLength,
        int? capacityWeight,
        long? rocketStatusId,
        int page,
        int pageSize,
        string sortBy,
        string sortDir
    );
}
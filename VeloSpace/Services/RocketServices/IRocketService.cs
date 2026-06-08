using VeloSpace.DTOs.Page;
using VeloSpace.DTOs.RocketDTOS;

namespace VeloSpace.Services.RocketServices;

public interface IRocketService
{
    Task<IEnumerable<RocketDTO>> GetAllAsync();
    Task<RocketDTO> GetByIdAsync(long id);
    Task AddAsync(RocketDTO rocketDto);
    Task UpdateAsync(long id, RocketDTO rocketDto);
    Task DeleteAsync(long id);
    Task<PagedResult<RocketDTO>> SearchAsync(
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
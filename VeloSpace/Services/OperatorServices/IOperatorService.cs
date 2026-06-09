using VeloSpace.DTOs.OperatorDTOS;
using VeloSpace.DTOs.Page;

namespace VeloSpace.Services.OperatorServices;

public interface IOperatorService
{
    Task<IEnumerable<OperatorRequestDTO>> GetAllAsync();
    Task<OperatorRequestDTO> GetByIdAsync(long id);
    Task AddAsync(OperatorRequestDTO operatorRequestDto);
    Task UpdateAsync(long id, OperatorDTO operatorDto);
    Task DeleteAsync(long id);
    Task<PagedResult<OperatorDTO>> SearchAsync(
        string? name,
        string? cpf,
        long? operatorStatusId,
        long? launchProviderId,
        int page,
        int pageSize,
        string sortBy,
        string sortDir
    );
}
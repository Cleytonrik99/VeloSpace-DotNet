using VeloSpace.DTOs.LaunchProvidersDTOS;
using VeloSpace.DTOs.Page;

namespace VeloSpace.Services.LaunchProvidersServices;

public interface ILaunchProvidersService
{
    Task<IEnumerable<LaunchProviderRequestDTO>> GetAllAsync();
    
    Task<LaunchProviderRequestDTO> GetByIdAsync(long id);
    
    Task AddAsync(LaunchProviderRequestDTO launchProviderRequestDto);
    
    Task UpdateAsync(long id, LaunchProviderDTO launchProviderDto);
    
    Task DeleteAsync(long id);
    
    Task<PagedResult<LaunchProviderDTO>> SearchAsync(
        string? corporateName,
        string? cnpj,
        int page,
        int pageSize,
        string sortBy,
        string sortDir
    );
}
using VeloSpace.DTOs.Page;
using VeloSpace.DTOs.Shippers;

namespace VeloSpace.Services.ShippersServices;

public interface IShipperService
{
    Task<IEnumerable<ShipperRequestDTO>> GetAllAsync();
    Task<ShipperRequestDTO> GetByIdAsync(long id);
    Task AddAsync(ShipperRequestDTO shipperRequestDto);
    Task UpdateAsync(long id, ShipperDTO shipperDto);
    Task DeleteAsync(long id);
    Task<PagedResult<ShipperDTO>> SearchAsync(
        string? name,
        string? type,
        int page,
        int pageSize,
        string sortBy,
        string sortDir
    );
}
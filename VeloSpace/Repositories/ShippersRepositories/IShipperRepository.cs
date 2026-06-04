using VeloSpace.DTOs.Shippers;

namespace VeloSpace.Repositories.ShippersRepositories;

public interface IShipperRepository
{
    
    Task<IEnumerable<ShipperRequestDTO>> GetAllAsync();
    
    Task<ShipperRequestDTO> GetByIdAsync(long id);
    
    Task AddAsync(ShipperRequestDTO shipperRequestDto);
    
    Task UpdateAsync(ShipperRequestDTO shipperRequestDto);
    
    Task DeleteAsync(long id);
    
    Task<(IEnumerable<Model.ShipperShi.Shipper> Items, int TotalItems)> SearchAsync(
        
    );

}
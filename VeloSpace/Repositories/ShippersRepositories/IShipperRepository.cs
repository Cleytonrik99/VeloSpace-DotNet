using VeloSpace.DTOs.Shippers;
using VeloSpace.Model.ShipperShi;

namespace VeloSpace.Repositories.ShippersRepositories;

public interface IShipperRepository
{
    
    Task<IEnumerable<Shipper>> GetAllAsync();
    
    Task<Shipper> GetByIdAsync(long id);
    
    Task AddAsync(Shipper shipper);
    
    Task UpdateAsync(Shipper shipper);
    
    Task DeleteAsync(long id);
    
    Task<(IEnumerable<Shipper> Items, int TotalItems)> SearchAsync(
        string? name,
        string? type,
        int page,
        int pageSize,
        string sortBy,
        string sortDir
    );

}
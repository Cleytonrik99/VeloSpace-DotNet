using VeloSpace.DTOs;
using VeloSpace.DTOs.Page;
using VeloSpace.DTOs.Shippers;
using VeloSpace.Repositories.ShippersRepositories;
using VeloSpace.Repositories.UsersRepositories;

namespace VeloSpace.Services.ShippersServices;

public class ShipperService : IShipperService
{
    private readonly IShipperRepository _shipperRepository;
    private readonly IUserAccountRepository _userAccountRepository;

    public ShipperService(IShipperRepository shipperRepository, IUserAccountRepository userAccountRepository)
    {
        _shipperRepository = shipperRepository;
        _userAccountRepository = userAccountRepository;
    }
    
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) {} // status code 404
    }
    
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) {} // status code 409
    }

    public Task<IEnumerable<ShipperRequestDTO>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ShipperRequestDTO> GetByIdAsync(long id)
    {
        var shipperResearch = await _shipperRepository.GetByIdAsync(id);
        var userResearch = await _userAccountRepository.GetByIdAsync(shipperResearch.UserAccountId);

        if (shipperResearch == null!)
        {
            throw new NotFoundException($"Shipper with id {id} Not Found.");
        }

        var shipperNewDto = new ShipperDTO
        {
            Name = shipperResearch.Name,
            ShipperDocument = shipperResearch.ShipperDocument,
            ShipperId = shipperResearch.ShipperId,
            Type = shipperResearch.Type,
            UserAccountId = shipperResearch.UserAccountId
        };

        var userNewDto = new UserAccountDTO
        {
            Email = userResearch.Email,
            Phone = userResearch.Phone,
            UserAccountId = userResearch.UserAccountId,
            UserRoleId = userResearch.UserRoleId
        };
        
        return new ShipperRequestDTO
        {
            ShipperDto = shipperNewDto,
            UserAccountDto = userNewDto
        };
    }

    public Task AddAsync(ShipperRequestDTO shipperRequestDto)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(long id, ShipperRequestDTO shipperRequestDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(long id)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResult<ShipperDTO>> SearchAsync(string? name, string? type, int page, int pageSize, string sortBy, string sortDir)
    {
        throw new NotImplementedException();
    }
}
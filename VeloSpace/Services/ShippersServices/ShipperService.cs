using Microsoft.EntityFrameworkCore;
using VeloSpace.Context;
using VeloSpace.DTOs;
using VeloSpace.DTOs.Page;
using VeloSpace.DTOs.Shippers;
using VeloSpace.Model.ShipperShi;
using VeloSpace.Model.User;
using VeloSpace.Repositories.ShippersRepositories;
using VeloSpace.Repositories.UsersRepositories;

namespace VeloSpace.Services.ShippersServices;

public class ShipperService : IShipperService
{
    private readonly IShipperRepository _shipperRepository;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly VeloSpaceContext _context;

    public ShipperService(IShipperRepository shipperRepository, IUserAccountRepository userAccountRepository, VeloSpaceContext context)
    {
        _shipperRepository = shipperRepository;
        _userAccountRepository = userAccountRepository;
        _context = context;
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

    public async Task AddAsync(ShipperRequestDTO shipperRequestDto)
    {
        var shipperNewDTO = shipperRequestDto.ShipperDto;

        var userNewDTO = shipperRequestDto.UserAccountDto;

        var searchUserEmail = await _context.UserAccount.FirstOrDefaultAsync(u => u.Email == userNewDTO.Email);

        if (searchUserEmail != null) throw new ConflictException("Email already registered");

        var newUser = new UserAccount
        {
            Email = userNewDTO.Email,
            HashedPassword = userNewDTO.HashedPassword,
            Phone = userNewDTO.Phone,
            UserRoleId = userNewDTO.UserRoleId
        };

        await _userAccountRepository.AddAsync(newUser);

        var newShipper = new Shipper
        {
            Name = shipperNewDTO.Name,
            ShipperDocument = shipperNewDTO.ShipperDocument,
            Type = shipperNewDTO.Type,
            UserAccountId = newUser.UserAccountId
        };

        await _shipperRepository.AddAsync(newShipper);
    }

    public async Task UpdateAsync(long id, ShipperDTO shipperDto)
    {
        var existingShipper = await _shipperRepository.GetByIdAsync(id);

        if (existingShipper == null) throw new NotFoundException($"Shipper with id {id} not found");

        shipperDto.ShipperId = id;

        existingShipper.Name = shipperDto.Name;
        existingShipper.ShipperDocument = shipperDto.ShipperDocument;
        existingShipper.Type = shipperDto.Type;

        await _shipperRepository.UpdateAsync(existingShipper);
    }

    public async Task DeleteAsync(long id)
    {
        var searchShipper = await _shipperRepository.GetByIdAsync(id);

        if (searchShipper == null) throw new NotFoundException($"Shipper with Id {id} not found");

        var searchUser = await _userAccountRepository.GetByIdAsync(searchShipper.UserAccountId);

        await _shipperRepository.DeleteAsync(id);

        if (searchUser != null) await _userAccountRepository.DeleteAsync(searchUser.UserAccountId);
    }

    public async Task<PagedResult<ShipperDTO>> SearchAsync(string? name, string? type, int page, int pageSize, string sortBy, string sortDir)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var (items, total) = await _shipperRepository.SearchAsync(name, type, page, pageSize, sortBy ?? "shipperId", sortDir ?? "asc");

        var dtoItems = items.Select(shipper => new ShipperDTO
        {
            Name = shipper.Name,
            ShipperDocument = shipper.ShipperDocument,
            ShipperId = shipper.ShipperId,
            Type = shipper.Type,
            UserAccountId = shipper.UserAccountId
        }).ToList();
        
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);

        return new PagedResult<ShipperDTO>
        {
            Items = dtoItems,
            PageInfo = new PageInfo
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = total,
                TotalPages = totalPages
            }
        };
    }
}
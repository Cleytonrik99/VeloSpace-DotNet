using Microsoft.EntityFrameworkCore;
using VeloSpace.Context;
using VeloSpace.DTOs;
using VeloSpace.DTOs.LaunchProvidersDTOS;
using VeloSpace.DTOs.Page;
using VeloSpace.Model.Launch;
using VeloSpace.Model.User;
using VeloSpace.Repositories.LaunchProvidersRepositories;
using VeloSpace.Repositories.UsersRepositories;

namespace VeloSpace.Services.LaunchProvidersServices;

public class LaunchProvidersService : ILaunchProvidersService
{
    private readonly ILaunchProvidersRepository _launchProvidersRepository;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly VeloSpaceContext _context;

    public LaunchProvidersService(ILaunchProvidersRepository launchProvidersRepository, IUserAccountRepository userAccountRepository, VeloSpaceContext context)
    {
        _launchProvidersRepository = launchProvidersRepository;
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

    public async Task<IEnumerable<LaunchProviderRequestDTO>> GetAllAsync()
    {
        var getLaunchers = await _launchProvidersRepository.GetAllAsync();
        var getUsers = await _userAccountRepository.GetAllAsync();

        var result = getLaunchers.Join(
            getUsers,
            launchProvider => launchProvider.UserAccountId,
            user => user.UserAccountId,
            (launchProvider, user) => new LaunchProviderRequestDTO
            {
                LaunchProviderDto = new LaunchProviderDTO
                {
                    LaunchProviderId = launchProvider.LaunchProviderId,
                    Cnpj = launchProvider.Cnpj,
                    CorporateName = launchProvider.CorporateName
                },

                UserAccountDto = new UserAccountDTO
                {
                    UserAccountId = user.UserAccountId,
                    Email = user.Email,
                    Phone = user.Phone,
                    UserRoleId = user.UserRoleId
                }
            }).ToList();

        return result;
    }

    public async Task<LaunchProviderRequestDTO> GetByIdAsync(long id)
    {
        var launchProviderResearch = await _launchProvidersRepository.GetByIdAsync(id);

        if (launchProviderResearch == null)
        {
            throw new NotFoundException($"Launch Provider with id {id} not found.");
        }

        var userResearch = await _userAccountRepository.GetByIdAsync(launchProviderResearch.UserAccountId);

        if (userResearch == null)
        {
            throw new NotFoundException($"User account linked to Launch Provider with id {id} not found.");
        }

        var launchProviderNewDto = new LaunchProviderDTO
        {
            LaunchProviderId = launchProviderResearch.LaunchProviderId,
            Cnpj = launchProviderResearch.Cnpj,
            CorporateName = launchProviderResearch.CorporateName,
            UserAccountId = launchProviderResearch.UserAccountId
        };

        var userNewDto = new UserAccountDTO
        {
            Email = userResearch.Email,
            Phone = userResearch.Phone,
            UserAccountId = userResearch.UserAccountId,
            UserRoleId = userResearch.UserRoleId
        };

        return new LaunchProviderRequestDTO
        {
            LaunchProviderDto = launchProviderNewDto,
            UserAccountDto = userNewDto
        };
    }

    public async Task AddAsync(LaunchProviderRequestDTO launchProviderRequestDto)
    {
        var launchProviderNewDTO = launchProviderRequestDto.LaunchProviderDto;

        var userNewDTO = launchProviderRequestDto.UserAccountDto;

        var searchUserEmail = await _context.UserAccount.FirstOrDefaultAsync(u => u.Email == userNewDTO.Email);
        
        if (searchUserEmail != null) throw new ConflictException("Email already registered");
        
        var searchCnpj = await _context.LaunchProvider
            .FirstOrDefaultAsync(lp => lp.Cnpj == launchProviderNewDTO.Cnpj);

        if (searchCnpj != null)
            throw new ConflictException("CNPJ already registered");
        
        var newUser = new UserAccount
        {
            Email = userNewDTO.Email,
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(launchProviderRequestDto.UserAccountDto.HashedPassword),
            Phone = userNewDTO.Phone,
            UserRoleId = userNewDTO.UserRoleId
        };
        
        await _userAccountRepository.AddAsync(newUser);

        var newLaunchProvider = new LaunchProvider
        {
            Cnpj = launchProviderNewDTO.Cnpj,
            CorporateName = launchProviderNewDTO.CorporateName,
            UserAccountId = newUser.UserAccountId
        };

        await _launchProvidersRepository.AddAsync(newLaunchProvider);
    }

    public async Task UpdateAsync(long id, LaunchProviderDTO launchProviderDto)
    {
        if (string.IsNullOrWhiteSpace(launchProviderDto.CorporateName)) throw new ArgumentException("Name can't be null");

        if (string.IsNullOrWhiteSpace(launchProviderDto.Cnpj)) throw new ArgumentException("Cnpj can't be null");

        var existingLaunchProvider = await _launchProvidersRepository.GetByIdAsync(id);

        if (existingLaunchProvider == null) throw new NotFoundException($"Launch Provider with id {id} not found");

        launchProviderDto.LaunchProviderId = id;

        existingLaunchProvider.Cnpj = launchProviderDto.Cnpj;
        existingLaunchProvider.CorporateName = launchProviderDto.CorporateName;

        await _launchProvidersRepository.UpdateAsync(existingLaunchProvider);
    }

    public async Task DeleteAsync(long id)
    {
        var searchLaunchProvider = await _launchProvidersRepository.GetByIdAsync(id);

        if (searchLaunchProvider == null) throw new NotFoundException($"Launch Provider with id {id} not found");
        
        var searchUser = await _userAccountRepository.GetByIdAsync(searchLaunchProvider.UserAccountId);

        await _launchProvidersRepository.DeleteAsync(id);

        if (searchUser != null) await _userAccountRepository.DeleteAsync(searchUser.UserAccountId);
    }

    public async Task<PagedResult<LaunchProviderDTO>> SearchAsync(string? corporateName, string? cnpj, int page, int pageSize, string sortBy, string sortDir)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var (items, total) = await _launchProvidersRepository.SearchAsync(corporateName, cnpj, page, pageSize, sortBy, sortDir);

        var dtoItems = items.Select(launchProvider => new LaunchProviderDTO
        {
            Cnpj = launchProvider.Cnpj,
            CorporateName = launchProvider.CorporateName,
            LaunchProviderId = launchProvider.LaunchProviderId,
            UserAccountId = launchProvider.UserAccountId
        }).ToList();
        
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);

        return new PagedResult<LaunchProviderDTO>
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
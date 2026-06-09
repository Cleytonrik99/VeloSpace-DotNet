using Microsoft.EntityFrameworkCore;
using VeloSpace.Context;
using VeloSpace.DTOs;
using VeloSpace.DTOs.OperatorDTOS;
using VeloSpace.DTOs.Page;
using VeloSpace.Model.OperatorShi;
using VeloSpace.Model.User;
using VeloSpace.Repositories.OperatorsRepositories;
using VeloSpace.Repositories.UsersRepositories;

namespace VeloSpace.Services.OperatorServices;

public class OperatorService : IOperatorService
{
    private readonly IOperatorRepository _operatorRepository;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly VeloSpaceContext _context;

    public OperatorService(IOperatorRepository operatorRepository, IUserAccountRepository userAccountRepository, VeloSpaceContext context)
    {
        _operatorRepository = operatorRepository;
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

    public async Task<IEnumerable<OperatorRequestDTO>> GetAllAsync()
    {
        var getOperators = await _operatorRepository.GetAllAsync();
        var getUsers = await _userAccountRepository.GetAllAsync();
        
        var result = getOperators.Join(
            getUsers,
            operatorNew => operatorNew.UserAccountId,
            user => user.UserAccountId,
            (operatorNew, user) => new OperatorRequestDTO
            {
                OperatorDto = new OperatorDTO
                {
                    Cpf = operatorNew.Cpf,
                    LaunchProviderId = operatorNew.LaunchProviderId,
                    Name = operatorNew.Name,
                    OperatorId = operatorNew.OperatorId,
                    OperatorStatusId = operatorNew.OperatorStatusId
                },

                UserAccountDto = new UserAccountDTO
                {
                    UserAccountId = user.UserAccountId,
                    Email = user.Email,
                    Phone = user.Phone,
                    UserRoleId = user.UserRoleId
                }
            }
        ).ToList();

        return result;
    }

    public async Task<OperatorRequestDTO> GetByIdAsync(long id)
    {
        var operatorResearch = await _operatorRepository.GetByIdAsync(id);

        if (operatorResearch == null)
            throw new NotFoundException($"Operator with id {id} not found");

        var userResearch = await _userAccountRepository.GetByIdAsync(operatorResearch.UserAccountId);

        if (userResearch == null)
            throw new NotFoundException($"User account linked to operator with id {id} not found");

        var operatorNewDto = new OperatorDTO
        {
            Name = operatorResearch.Name,
            Cpf = operatorResearch.Cpf,
            LaunchProviderId = operatorResearch.LaunchProviderId,
            OperatorId = operatorResearch.OperatorId,
            OperatorStatusId = operatorResearch.OperatorStatusId
        };

        var userNewDto = new UserAccountDTO
        {
            Email = userResearch.Email,
            Phone = userResearch.Phone,
            UserAccountId = userResearch.UserAccountId,
            UserRoleId = userResearch.UserRoleId
        };

        return new OperatorRequestDTO
        {
            OperatorDto = operatorNewDto,
            UserAccountDto = userNewDto
        };
    }

    public async Task AddAsync(OperatorRequestDTO operatorRequestDto)
    {
        var operatorNewDTO = operatorRequestDto.OperatorDto;
        var userNewDTO = operatorRequestDto.UserAccountDto;

        var searchUserEmail = await _context.UserAccount
            .FirstOrDefaultAsync(u => u.Email == userNewDTO.Email);

        if (searchUserEmail != null)
            throw new ConflictException("Email already registered");

        var searchCpf = await _context.Operator
            .FirstOrDefaultAsync(o => o.Cpf == operatorNewDTO.Cpf);

        if (searchCpf != null)
            throw new ConflictException("CPF already registered");
        
        var launchProviderExists = await _context.LaunchProvider
            .AnyAsync(lp => lp.LaunchProviderId == operatorNewDTO.LaunchProviderId);

        if (!launchProviderExists)
            throw new NotFoundException("Launch Provider not found");

        var statusExists = await _context.OperatorStatus
            .AnyAsync(os => os.OperatorStatusId == operatorNewDTO.OperatorStatusId);

        if (!statusExists)
            throw new NotFoundException("Operator Status not found");

        var newUser = new UserAccount
        {
            Email = userNewDTO.Email,
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(userNewDTO.HashedPassword),
            Phone = userNewDTO.Phone,
            UserRoleId = userNewDTO.UserRoleId
        };

        await _userAccountRepository.AddAsync(newUser);

        var operatorNew = new Operator
        {
            Name = operatorNewDTO.Name,
            Cpf = operatorNewDTO.Cpf,
            LaunchProviderId = operatorNewDTO.LaunchProviderId,
            OperatorStatusId = operatorNewDTO.OperatorStatusId,
            UserAccountId = newUser.UserAccountId
        };

        await _operatorRepository.AddAsync(operatorNew);
    }

    public async Task UpdateAsync(long id, OperatorDTO operatorDto)
    {
        if (string.IsNullOrWhiteSpace(operatorDto.Name)) throw new ArgumentException("Name can't be null");
        
        if (string.IsNullOrWhiteSpace(operatorDto.Cpf))
            throw new ArgumentException("Cpf can't be null");
        
        if (operatorDto.OperatorStatusId == 0) throw new ArgumentException("Operator Status Id can't be null");
        
        if (operatorDto.LaunchProviderId == 0) throw new ArgumentException("Launch Provider Id can't be null");

        var existingOperator = await _operatorRepository.GetByIdAsync(id);
        
        if (existingOperator == null) throw new NotFoundException($"Operator with id {id} not found");

        operatorDto.OperatorId = id;

        existingOperator.Name = operatorDto.Name;
        existingOperator.Cpf = operatorDto.Cpf;
        existingOperator.OperatorStatusId = operatorDto.OperatorStatusId;
        existingOperator.LaunchProviderId = operatorDto.LaunchProviderId;

        await _operatorRepository.UpdateAsync(existingOperator);
    }

    public async Task DeleteAsync(long id)
    {
        var existingOperator = await _operatorRepository.GetByIdAsync(id);
        
        if (existingOperator == null) throw new NotFoundException($"Operator with id {id} not found");
        
        var searchUser = await _userAccountRepository.GetByIdAsync(existingOperator.UserAccountId);

        await _operatorRepository.DeleteAsync(id);
        
        if (searchUser != null) await _userAccountRepository.DeleteAsync(searchUser.UserAccountId);
    }

    public async Task<PagedResult<OperatorDTO>> SearchAsync(string? name, string? cpf, long? operatorStatusId, long? launchProviderId, int page, int pageSize, string sortBy, string sortDir)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var (items, total) = await _operatorRepository.SearchAsync(name, cpf, operatorStatusId, launchProviderId, page, pageSize, sortBy, sortDir);

        var dtoItems = items.Select(operatorNew => new OperatorDTO
        {
            Name = operatorNew.Name,
            Cpf = operatorNew.Cpf,
            LaunchProviderId = operatorNew.LaunchProviderId,
            OperatorId = operatorNew.OperatorId,
            OperatorStatusId = operatorNew.OperatorStatusId,
        }).ToList();
        
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);

        return new PagedResult<OperatorDTO>
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
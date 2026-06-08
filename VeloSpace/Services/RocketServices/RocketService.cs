using Microsoft.EntityFrameworkCore;
using VeloSpace.Context;
using VeloSpace.DTOs.Page;
using VeloSpace.DTOs.RocketDTOS;
using VeloSpace.Model.RocketShi;
using VeloSpace.Repositories.RocketRepositories;

namespace VeloSpace.Services.RocketServices;

public class RocketService : IRocketService
{
    private readonly IRocketRepository _rocketRepository;
    private readonly VeloSpaceContext _context;

    public RocketService(IRocketRepository rocketRepository, VeloSpaceContext context)
    {
        _rocketRepository = rocketRepository;
        _context = context;
    }
    
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) {} // status code 404
    }

    public async Task<IEnumerable<RocketDTO>> GetAllAsync()
    {
        var getRockets = await _rocketRepository.GetAllAsync();

        return getRockets.Select(rocket => new RocketDTO
        {
            CapacityHeight = rocket.CapacityHeight,
            CapacityLength = rocket.CapacityLength,
            CapacityWeight = rocket.CapacityWeight,
            CapacityWidth = rocket.CapacityWidth,
            LaunchDate = rocket.LaunchDate,
            Name = rocket.Name,
            RocketStatusId = rocket.RocketStatusId
        }).ToList();
    }

    public async Task<RocketDTO> GetByIdAsync(long id)
    {
        var rocketResearch = await _rocketRepository.GetByIdAsync(id);
        
        if (rocketResearch == null) throw new NotFoundException($"Rocket with id {id} Not Found.");

        return new RocketDTO
        {
            CapacityHeight = rocketResearch.CapacityHeight,
            CapacityLength = rocketResearch.CapacityLength,
            CapacityWeight = rocketResearch.CapacityWeight,
            CapacityWidth = rocketResearch.CapacityWidth,
            LaunchDate = rocketResearch.LaunchDate,
            Name = rocketResearch.Name,
            RocketId = rocketResearch.RocketId,
            RocketStatusId = rocketResearch.RocketStatusId
        };
    }

    public async Task AddAsync(RocketDTO rocketDto)
    {
        var newRocket = new Rocket
        {
            CapacityHeight = rocketDto.CapacityHeight,
            CapacityLength = rocketDto.CapacityLength,
            CapacityWeight = rocketDto.CapacityWeight,
            CapacityWidth = rocketDto.CapacityWidth,
            LaunchDate = DateTime.Now,
            Name = rocketDto.Name,
            RocketId = rocketDto.RocketId,
            RocketStatusId = rocketDto.RocketStatusId
        };
        
        var rocketStatusExists = await _context.RocketStatus
            .CountAsync(rs => rs.RocketStatusId == rocketDto.RocketStatusId) > 0;
        
        if (!rocketStatusExists)
        {
            throw new NotFoundException("Rocket Status not found.");
        }

        await _rocketRepository.AddAsync(newRocket);
    }

    public async Task UpdateAsync(long id, RocketDTO rocketDto)
    {
        if (string.IsNullOrWhiteSpace(rocketDto.Name))
            throw new ArgumentException("Name can't be null");

        if (rocketDto.CapacityHeight <= 0)
            throw new ArgumentException("Capacity Height must be greater than zero");

        if (rocketDto.CapacityWidth <= 0)
            throw new ArgumentException("Capacity Width must be greater than zero");

        if (rocketDto.CapacityLength <= 0)
            throw new ArgumentException("Capacity Length must be greater than zero");

        if (rocketDto.CapacityWeight <= 0)
            throw new ArgumentException("Capacity Weight must be greater than zero");

        if (rocketDto.RocketStatusId <= 0)
            throw new ArgumentException("Rocket Status Id can't be null");

        var existingRocket = await _rocketRepository.GetByIdAsync(id);

        if (existingRocket == null)
            throw new NotFoundException($"Rocket with id {id} not found");

        existingRocket.Name = rocketDto.Name;
        existingRocket.CapacityHeight = rocketDto.CapacityHeight;
        existingRocket.CapacityWidth = rocketDto.CapacityWidth;
        existingRocket.CapacityLength = rocketDto.CapacityLength;
        existingRocket.CapacityWeight = rocketDto.CapacityWeight;
        existingRocket.RocketStatusId = rocketDto.RocketStatusId;

        await _rocketRepository.UpdateAsync(existingRocket);
    }

    public async Task DeleteAsync(long id)
    {
        var existingRocket = await _rocketRepository.GetByIdAsync(id);
        
        if (existingRocket == null) throw new NotFoundException($"Rocket with id {id} not found");

        await _rocketRepository.DeleteAsync(id);
    }

    public async Task<PagedResult<RocketDTO>> SearchAsync(string? name, int? capacityHeight, int? capacityWidth, int? capacityLength, int? capacityWeight, long? rocketStatusId, int page, int pageSize, string sortBy, string sortDir)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var (items, total) = await _rocketRepository.SearchAsync(name, capacityHeight, capacityWidth, capacityLength, capacityWeight, rocketStatusId, page, pageSize, sortBy, sortDir);

        var dtoItems = items.Select(rocket => new RocketDTO
        {
            CapacityHeight = rocket.CapacityHeight,
            CapacityLength = rocket.CapacityLength,
            CapacityWeight = rocket.CapacityWeight,
            CapacityWidth = rocket.CapacityWidth,
            LaunchDate = rocket.LaunchDate,
            Name = rocket.Name,
            RocketId = rocket.RocketId,
            RocketStatusId = rocket.RocketStatusId
        }).ToList();
        
        var totalPages = (int)Math.Ceiling(total / (double)pageSize);

        return new PagedResult<RocketDTO>
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
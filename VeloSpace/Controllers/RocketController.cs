using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeloSpace.DTOs.RocketDTOS;
using VeloSpace.Services.RocketServices;

namespace VeloSpace.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RocketController : ControllerBase
{
    private readonly IRocketService _rocketService;

    public RocketController(IRocketService rocketService)
    {
        _rocketService = rocketService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var getAllShippers = await _rocketService.GetAllAsync();

            return Ok(new {items = getAllShippers} );
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal error when searching for the Rockets",
                details = ex.Message
            });
        }
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var getShipperId = await _rocketService.GetByIdAsync(id);

            return Ok(getShipperId);
        }
        catch (RocketService.NotFoundException ntex)
        {
            return NotFound(ntex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal error when searching for the Rocket",
                details = ex.Message
            });
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddRocket([FromBody] RocketDTO rocketDto)
    {
        try
        {
            await _rocketService.AddAsync(rocketDto);

            return Created("", new {message = "Rocket added sucessfully"});
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new {message = "Missing informations in the request body"});
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal error when adding the Rocket",
                details = ex.Message
            });
        }
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRocket(long id, [FromBody] RocketDTO rocketDto)
    {
        try
        {
            await _rocketService.UpdateAsync(id, rocketDto);

            return Ok(new {message = $"Rocket with id {id} updated sucessfully"});
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (RocketService.NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal error when updating the Rocket",
                details = ex.Message
            });
        }
    }
    
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteRocket(long id)
    {
        try
        {
            await _rocketService.DeleteAsync(id);

            return Ok(new { message = $"Rocket with id {id} deleted" });
        }
        catch (RocketService.NotFoundException ex)
        {
            return NotFound(new { message = $"Rocket with id {id} not found" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = $"Internal error when deleting the Rocket",
                details = ex.Message
            });
        }
    }

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchRocket(
        [FromQuery] string? name,
        [FromQuery] int? capacityHeight,
        [FromQuery] int? capacityWidth,
        [FromQuery] int? capacityLength,
        [FromQuery] int? capacityWeight,
        [FromQuery] long? rocketStatusId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortBy = "rocketId",
        [FromQuery] string sortDir = "asc"
    )
    {
        try
        {
            var result = await _rocketService.SearchAsync(name, capacityHeight, capacityWidth, capacityLength, capacityWeight, rocketStatusId, page, pageSize, sortBy, sortDir);

            return Ok(new { Items = result.Items, PageInfo = result.PageInfo });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal error when searching the Rockets",
                details = ex.Message
            });
        }
    }
}
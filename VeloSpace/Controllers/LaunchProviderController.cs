using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeloSpace.DTOs.LaunchProvidersDTOS;
using VeloSpace.Services.LaunchProvidersServices;

namespace VeloSpace.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LaunchProviderController : ControllerBase
{
    private readonly ILaunchProvidersService _launchProvidersService;

    public LaunchProviderController(ILaunchProvidersService launchProvidersService)
    {
        _launchProvidersService = launchProvidersService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var getAllShippers = await _launchProvidersService.GetAllAsync();

            return Ok(new {items = getAllShippers} );
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal error when searching for the Launch Providers",
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
            var getShipperId = await _launchProvidersService.GetByIdAsync(id);

            return Ok(getShipperId);
        }
        catch (LaunchProvidersService.NotFoundException ntex)
        {
            return NotFound(ntex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal error when searching for the Launch Providers",
                details = ex.Message
            });
        }
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddLaunchProvider([FromBody] LaunchProviderRequestDTO launchProviderRequestDto)
    {
        try
        {
            await _launchProvidersService.AddAsync(launchProviderRequestDto);

            return Created("", new {message = "Shipper added sucessfully"});
        }
        catch (LaunchProvidersService.ConflictException ex)
        {
            return Conflict(ex.Message);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new {message = "Missing informations in the request body"});
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal error when adding the Launch Provider",
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
    public async Task<IActionResult> UpdateLaunchProvider(long id, [FromBody] LaunchProviderDTO launchProviderDto)
    {
        try
        {
            await _launchProvidersService.UpdateAsync(id, launchProviderDto);

            return Ok(new {message = $"Launch Provider with id {id} updated sucessfully"});
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (LaunchProvidersService.NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal error when updating the Launch Provider",
                details = ex.Message
            });
        }
    }
    
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteLaunchProvider(long id)
    {
        try
        {
            await _launchProvidersService.DeleteAsync(id);

            return Ok(new { message = $"Launch Provider with id {id} deleted" });
        }
        catch (LaunchProvidersService.NotFoundException ex)
        {
            return NotFound(new { message = $"Launch Provider with id {id} not found" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = $"Internal error when deleting the Launch Provider",
                details = ex.Message
            });
        }
    }
    
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchLaunchProvider(
        [FromQuery] string? corporateName,
        [FromQuery] string? cnpj,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortBy = "launchProviderId",
        [FromQuery] string sortDir = "asc"
    )
    {
        try
        {
            var result = await _launchProvidersService.SearchAsync(corporateName, cnpj, page, pageSize, sortBy, sortDir);

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
                message = "Internal error when searching the Launch Provider",
                details = ex.Message
            });
        }
    }
}
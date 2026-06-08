using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeloSpace.DTOs.Shippers;
using VeloSpace.Services.ShippersServices;

namespace VeloSpace.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShipperController : ControllerBase
{
    private readonly IShipperService _shipperService;

    public ShipperController(IShipperService shipperService)
    {
        _shipperService = shipperService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var getAllShippers = await _shipperService.GetAllAsync();

            return Ok(new {items = getAllShippers} );
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal error when searching for the Shippers",
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
            var getShipperId = await _shipperService.GetByIdAsync(id);

            return Ok(getShipperId);
        }
        catch (ShipperService.NotFoundException ntex)
        {
            return NotFound(ntex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal error when searching for the Shipper",
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
    public async Task<IActionResult> AddShipper([FromBody] ShipperRequestDTO shipperRequestDto)
    {
        try
        {
            await _shipperService.AddAsync(shipperRequestDto);

            return Created("", new {message = "Shipper added sucessfully"});
        }
        catch (ShipperService.ConflictException ex)
        {
            return Conflict(ex.Message);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal error when adding the Shipper",
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
    public async Task<IActionResult> UpdateShipper(long id, [FromBody] ShipperDTO shipperDto)
    {
        try
        {
            await _shipperService.UpdateAsync(id, shipperDto);

            return Ok(new {message = $"Shipper with id {id} updated sucessfully"});
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ShipperService.NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal error when updating the Shipper",
                details = ex.Message
            });
        }
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteShipper(long id)
    {
        try
        {
            await _shipperService.DeleteAsync(id);

            return Ok(new { message = $"Shipper with id {id} deleted" });
        }
        catch (ShipperService.NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = $"Internal error when deleting the Shipper",
                details = ex.Message
            });
        }
    }

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchShipper(
        [FromQuery] string? name,
        [FromQuery] string? type,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortBy = "shipperId",
        [FromQuery] string sortDir = "asc")
    {
        try
        {
            var result = await _shipperService.SearchAsync(name, type, page, pageSize, sortBy, sortDir);

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
                message = "Internal error when searching the Shipper",
                details = ex.Message
            });
        }
    }
}
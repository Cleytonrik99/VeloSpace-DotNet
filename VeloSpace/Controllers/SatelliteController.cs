using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeloSpace.Services.RocketServices;
using VeloSpace.Services.SatellitesServices;

namespace VeloSpace.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SatelliteController : ControllerBase
{
    private readonly ISatelliteService _satelliteService;

    public SatelliteController(ISatelliteService satelliteService)
    {
        _satelliteService = satelliteService;
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
            var getShipperId = await _satelliteService.GetByIdAsync(id);

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
                message = "Internal error when searching for the Satellite",
                details = ex.Message
            });
        }
    }
}
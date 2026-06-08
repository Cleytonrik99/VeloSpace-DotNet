using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    /// <summary>
    /// Gets a specific Satellite by ID.
    /// </summary>
    /// <param name="id">Satellite ID.</param>
    /// <remarks>
    /// Endpoint that returns a specific Satellite by its identifier.
    ///
    /// Possible status codes:
    /// - 200 OK: Satellite found and returned in the response body
    /// - 404 Not Found: no Satellite with the provided ID was found
    /// - 401 Unauthorized: authentication token is missing, invalid, or expired
    /// - 500 Internal Server Error: unexpected server error
    /// </remarks>
    /// <returns>Resource containing Satellite information.</returns>
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var getSatelliteId = await _satelliteService.GetByIdAsync(id);

            return Ok(getSatelliteId);
        }
        catch (SatelliteService.NotFoundException ntex)
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeloSpace.DTOs.OperatorDTOS;
using VeloSpace.Services.OperatorServices;

namespace VeloSpace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OperatorController : ControllerBase
{
    private readonly IOperatorService _operatorService;

    public OperatorController(IOperatorService operatorService)
    {
        _operatorService = operatorService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var getAllShippers = await _operatorService.GetAllAsync();

            return Ok(new {items = getAllShippers} );
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal error when searching for the Operators",
                details = ex.Message
            });
        }
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var getShipperId = await _operatorService.GetByIdAsync(id);

            return Ok(getShipperId);
        }
        catch (OperatorService.NotFoundException ntex)
        {
            return NotFound(ntex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal error when searching for the Operator",
                details = ex.Message
            });
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddOperator([FromBody] OperatorRequestDTO operatorRequestDto)
    {
        try
        {
            await _operatorService.AddAsync(operatorRequestDto);

            return Created("", new {message = "Operator added sucessfully"});
        }
        catch (OperatorService.ConflictException ex)
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
                message = "Internal error when adding the Operator",
                details = ex.Message
            });
        }
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateOperator(long id, [FromBody] OperatorDTO operatorDto)
    {
        try
        {
            await _operatorService.UpdateAsync(id, operatorDto);

            return Ok(new {message = $"Operator with id {id} updated sucessfully"});
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (OperatorService.NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal error when updating the Operator",
                details = ex.Message
            });
        }
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteOperator(long id)
    {
        try
        {
            await _operatorService.DeleteAsync(id);

            return Ok(new { message = $"Operator with id {id} deleted" });
        }
        catch (OperatorService.NotFoundException ex)
        {
            return NotFound(new { message = $"Operator with id {id} not found" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = $"Internal error when deleting the Operator",
                details = ex.Message
            });
        }
    }

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchOperator(
        [FromQuery] string? name,
        [FromQuery] int? cpf,
        [FromQuery] long? operatorStatusId,
        [FromQuery] long? launchProviderId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortBy = "operatorId",
        [FromQuery] string sortDir = "asc"
    )
    {
        try
        {
            var result = await _operatorService.SearchAsync(name, cpf, operatorStatusId, launchProviderId, page, pageSize, sortBy, sortDir);

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
                message = "Internal error when searching the Operators",
                details = ex.Message
            });
        }
    }
}
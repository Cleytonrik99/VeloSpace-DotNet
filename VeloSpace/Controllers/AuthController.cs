using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeloSpace.DTOs.Auth;
using VeloSpace.Services.Auth;

namespace VeloSpace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="loginRequest">User login credentials.</param>
    /// <remarks>
    /// Public endpoint used to authenticate a registered user in the VeloSpace API.
    ///
    /// This endpoint validates the user's email and password. If the credentials are valid,
    /// it returns the authentication response, usually containing the JWT token and user information.
    ///
    /// Expected body (JSON):
    /// <code>
    /// {
    ///     "email": "user@email.com",
    ///     "password": "userPassword"
    /// }
    /// </code>
    ///
    /// Possible status codes:
    /// - 200 OK: user authenticated successfully
    /// - 400 Bad Request: invalid or incomplete request body
    /// - 401 Unauthorized: invalid email or password
    /// - 500 Internal Server Error: unexpected error while authenticating the user
    /// </remarks>
    /// <returns>Authentication response containing the JWT token and user data.</returns>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
    {
        try
        {
            var response = await _authService.LoginAsync(loginRequest);

            return Ok(response);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new
            {
                message = "Invalid request body.",
                details = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Internal error when authenticating the user",
                details = ex.Message
            });
        }
    }
}
using VeloSpace.DTOs.Auth;

namespace VeloSpace.Services.Auth;

public interface IAuthService
{
    Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequest);
}
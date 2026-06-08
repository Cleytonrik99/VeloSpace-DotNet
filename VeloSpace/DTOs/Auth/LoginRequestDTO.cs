namespace VeloSpace.DTOs.Auth;

public class LoginRequestDTO
{
    public string Email { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
}
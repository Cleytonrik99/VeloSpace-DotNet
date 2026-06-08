namespace VeloSpace.DTOs.Auth;

public class LoginResponseDTO
{
    public string Token { get; set; }
    public long UserAccountId { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public long UserRoleId { get; set; }
}
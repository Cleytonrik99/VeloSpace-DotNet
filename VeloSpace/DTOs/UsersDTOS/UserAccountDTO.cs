namespace VeloSpace.DTOs;

public class UserAccountDTO
{
    public long UserAccountId { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
    public string Phone { get; set; }
    public long UserRoleId { get; set; }
}
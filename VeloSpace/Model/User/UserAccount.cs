using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSpace.Model.User;

[Table("USER_ACCOUNT")]
[Index(nameof(Email), IsUnique = true)]
public class UserAccount
{
    [Key]
    [Column("USER_ACCOUNT_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long UserAccountId { get; set; }
    
    [Required]
    [Column("EMAIL")]
    [StringLength(255)]
    public string Email { get; set; }
    
    [Required]
    [Column("HASHED_PASSWORD")]
    [StringLength(255)]
    public string HashedPassword { get; set; }
    
    [Required]
    [Column("PHONE")]
    [StringLength(15)]
    public string Phone { get; set; }
    
    [Required]
    [Column("USER_ROLE_ID")]
    public long UserRoleId { get; set; }
    
    [ForeignKey(nameof(UserRoleId))]
    public UserRole UserRole { get; set; }
}
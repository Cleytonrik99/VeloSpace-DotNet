using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSpace.Model.User;

[Table("USER_ROLE")]
[Index(nameof(Code), IsUnique = true)]
public class UserRole
{
    [Key]
    [Column("USER_ROLE_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long UserRoleId { get; set; }
    
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(55)]
    public string Description { get; set; }
    
    [Required]
    [Column("CODE")]
    public string Code { get; set; }
}
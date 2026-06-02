using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSpace.Model.Launch;

[Table("LAUNCH_PROVIDER")]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Cnpj), IsUnique = true)]
public class LaunchProvider
{
    [Key]
    [Column("LAUNCH_PROVIDER_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long LaunchProviderId { get; set; }
    
    [Required]
    [Column("CORPORATE_NAME")]
    [StringLength(40)]
    public string CorporateName { get; set; }
    
    [Required]
    [Column("CNPJ")]
    [StringLength(14)]
    public string Cnpj { get; set; }
    
    [Required]
    [Column("PHONE", TypeName = "numeric(15)")]
    public int Phone { get; set; }
    
    [Required]
    [Column("PASSWORD_HASH")]
    [StringLength(255)]
    public string PasswordHash { get; set; }
    
    [Required]
    [Column("EMAIL")]
    [StringLength(255)]
    public string Email { get; set; }
}
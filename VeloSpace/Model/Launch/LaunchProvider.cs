using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using VeloSpace.Model.User;

namespace VeloSpace.Model.Launch;

[Table("LAUNCH_PROVIDER")]
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
    [Column("USER_ACCOUNT_ID")]
    public long UserAccountId { get; set; }
    
    [ForeignKey(nameof(UserAccountId))]
    public UserAccount UserAccount { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using VeloSpace.Model.Launch;

namespace VeloSpace.Model.PayloadShi;

[Table("PAYLOAD_HANDLER")]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(Cpf), IsUnique = true)]
public class PayloadHandler
{
    [Key]
    [Column("PAYLOAD_HANDLER_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long PayloadHandlerId { get; set; }
    
    [Required]
    [Column("NAME")]
    [StringLength(40)]
    public string Name { get; set; }
    
    [Required]
    [Column("CPF", TypeName = "numeric(11)")]
    public int Cpf { get; set; }
    
    [Required]
    [Column("EMAIL")]
    [StringLength(255)]
    public string Email { get; set; }
    
    [Required]
    [Column("PHONE", TypeName = "numeric(15)")]
    public int Phone { get; set; }
    
    [Required]
    [Column("PASSWORD_HASH")]
    [StringLength(255)]
    public string PasswordHash { get; set; }
    
    [Required]
    [Column("HANDLER_STATUS_ID")]
    public long HandlerStatusId { get; set; }
    
    [ForeignKey(nameof(HandlerStatusId))]
    public HandlerStatus HandlerStatus { get; set; }
    
    [Required]
    [Column("LAUNCH_PROVIDER_ID")]
    public long LaunchProviderId { get; set; }
    
    [ForeignKey(nameof(LaunchProviderId))]
    public LaunchProvider LaunchProvider { get; set; }
}
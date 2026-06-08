using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using VeloSpace.Model.Launch;
using VeloSpace.Model.PayloadShi;
using VeloSpace.Model.User;

namespace VeloSpace.Model.OperatorShi;

[Table("VS_OPERATOR")]
[Index(nameof(Cpf), IsUnique = true)]
public class Operator
{
    [Key]
    [Column("OPERATOR_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long OperatorId { get; set; }
    
    [Required]
    [Column("NAME")]
    [StringLength(40)]
    public string Name { get; set; }
    
    [Required]
    [Column("CPF", TypeName = "numeric(11)")]
    public int Cpf { get; set; }
    
    [Required]
    [Column("OPERATOR_STATUS_ID")]
    public long OperatorStatusId { get; set; }
    
    [ForeignKey(nameof(OperatorStatusId))]
    public OperatorStatus OperatorStatus { get; set; }
    
    [Required]
    [Column("LAUNCH_PROVIDER_ID")]
    public long LaunchProviderId { get; set; }
    
    [ForeignKey(nameof(LaunchProviderId))]
    public LaunchProvider LaunchProvider { get; set; }
    
    [Required]
    [Column("USER_ACCOUNT_ID")]
    public long UserAccountId { get; set; }
    
    [ForeignKey(nameof(UserAccountId))]
    public UserAccount UserAccount { get; set; }
}    
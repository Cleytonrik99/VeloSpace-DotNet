using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSpace.Model.PayloadShi;

[Table("OPERATOR_STATUS")]
[Index(nameof(Code), IsUnique = true)]
public class OperatorStatus
{
    [Key]
    [Column("OPERATOR_STATUS_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long OperatorStatusId { get; set; }
    
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(30)]
    public string Description { get; set; }
    
    [Required]
    [Column("CODE")]
    [StringLength(55)]
    public string Code { get; set; }
}
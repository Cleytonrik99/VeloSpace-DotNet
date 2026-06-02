using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeloSpace.Model.PayloadShi;

[Table("PAYLOAD_PRIORITY")]
public class PayloadPriority
{
    [Key]
    [Column("PAYLOAD_PRIORITY_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long PayloadPriorityId { get; set; }
    
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(40)]
    public string Description { get; set; }
    
    [Required]
    [Column("LEVEL", TypeName = "numeric(1)")]
    public int Level { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSpace.Model.SatelliteShi;

[Table("PAYLOAD_PRIORITY")]
[Index(nameof(Level), IsUnique = true)]
public class SatellitePriority
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
    [Column("LEVEL", TypeName = "numeric(2)")]
    public int Level { get; set; }
}
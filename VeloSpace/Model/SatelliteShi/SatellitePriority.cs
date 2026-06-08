using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSpace.Model.SatelliteShi;

[Table("VS_SATELLITE_PRIORITY")]
[Index(nameof(Level), IsUnique = true)]
public class SatellitePriority
{
    [Key]
    [Column("SATELLITE_PRIORITY_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long SatellitePriorityId { get; set; }
    
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(40)]
    public string Description { get; set; }
    
    [Required]
    [Column("LEVEL", TypeName = "numeric(2)")]
    public int Level { get; set; }
}
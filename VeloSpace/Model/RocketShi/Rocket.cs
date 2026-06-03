using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeloSpace.Model.RocketShi;

[Table("ROCKET")]
public class Rocket
{
    [Key]
    [Column("ROCKET")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long RocketId { get; set; }
    
    [Required]
    [Column("NAME")]
    [StringLength(40)]
    public string Name { get; set; }
    
    [Required]
    [Column("CAPACITY_HEIGHT", TypeName = "numeric(5)")]
    public int CapacityHeight { get; set; }
    
    [Required]
    [Column("CAPACITY_WIDTH", TypeName = "numeric(5)")]
    public int CapacityWidth { get; set; }
    
    [Required]
    [Column("CAPACITY_LENGTH", TypeName = "numeric(5)")]
    public int CapacityLength { get; set; }
    
    [Required]
    [Column("CAPACITY_WEIGHT", TypeName = "numeric(5)")]
    public int CapacityWeight { get; set; }
    
    [Required]
    [Column("LAUNCH_DATE", TypeName = "Date")]
    public DateTime LaunchDate { get; set; }
    
    [Required]
    [Column("ROCKET_STATUS_ID")]
    public long RocketStatusId { get; set; }
    
    [ForeignKey(nameof(RocketStatusId))]
    public RocketStatus RocketStatus { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeloSpace.Model.RocketShi;

[Table("ROCKET_STATUS")]
public class RocketStatus
{
    [Key]
    [Column("ROCKET_STATUS_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long RocketStatusId { get; set; }
    
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(40)]
    public string Description { get; set; }
}
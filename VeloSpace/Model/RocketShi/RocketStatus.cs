using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSpace.Model.RocketShi;

[Table("ROCKET_STATUS")]
[Index(nameof(Code), IsUnique = true)]
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
    
    [Required]
    [Column("CODE")]
    [StringLength(55)]
    public string Code { get; set; }
}
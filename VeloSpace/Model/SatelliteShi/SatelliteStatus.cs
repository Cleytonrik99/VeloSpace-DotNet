using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSpace.Model.SatelliteShi;

[Table("SATELLITE_STATUS")]
[Index(nameof(Code), IsUnique = true)]
public class SatelliteStatus
{
    [Key]
    [Column("SATELLITE_STATUS_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long SatelliteStatusId { get; set; }
    
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(40)]
    public string Description { get; set; }
    
    [Required]
    [Column("CODE")]
    [StringLength(55)]
    public string Code { get; set; }
}
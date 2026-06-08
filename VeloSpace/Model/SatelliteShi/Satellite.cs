using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VeloSpace.Model.Launch;
using VeloSpace.Model.PayloadShi;
using VeloSpace.Model.RocketShi;
using VeloSpace.Model.ShipperShi;

namespace VeloSpace.Model.SatelliteShi;

[Table("VS_SATELLITE")]
public class Satellite
{
    [Key]
    [Column("SATELLITE_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long SatelliteId { get; set; }
    
    [Required]
    [Column("NAME")]
    [StringLength(55)]
    public string Name { get; set; }
    
    [Required]
    [Column("HEIGHT", TypeName = "numeric(5)")]
    public int Height { get; set; }
    
    [Required]
    [Column("WIDTH", TypeName = "numeric(5)")]
    public int Width { get; set; }
    
    [Required]
    [Column("LENGTH", TypeName = "numeric(5)")]
    public int Length { get; set; }
    
    [Required]
    [Column("WEIGHT", TypeName = "numeric(5)")]
    public int Weight { get; set; }
    
    [Column("TRACKING_CODE")]
    [StringLength(55)]
    public string TrackingCode { get; set; }
    
    [Required]
    [Column("LAUNCH_JUSTIFICATION")]
    [StringLength(500)]
    public string LaunchJustification { get; set; }
    
    [Column("SATELLITE_PRIORITY_ID")]
    public long SatellitePriorityId { get; set; }
    
    [ForeignKey(nameof(SatellitePriorityId))]
    public SatellitePriority SatellitePriority { get; set; }
    
    [Required]
    [Column("ROCKET_ID")]
    public long RocketId { get; set; }
    
    [ForeignKey(nameof(RocketId))]
    public Rocket Rocket { get; set; }
    
    [Required]
    [Column("SATELLITE_STATUS_ID")]
    public long SatelliteStatusId { get; set; }
    
    [ForeignKey(nameof(SatelliteStatusId))]
    public SatelliteStatus SatelliteStatus { get; set; }
    
    [Required]
    [Column("SHIPPER_ID")]
    public long ShipperId { get; set; }
    
    [ForeignKey(nameof(ShipperId))]
    public Shipper Shipper { get; set; }
    
    [Required]
    [Column("LAUNCH_PROVIDER_ID")]
    public long LaunchProviderId { get; set; }
    
    [ForeignKey(nameof(LaunchProviderId))]
    public LaunchProvider LaunchProvider { get; set; }
}
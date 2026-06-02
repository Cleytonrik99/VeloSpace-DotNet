using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VeloSpace.Model.Launch;
using VeloSpace.Model.RocketShi;
using VeloSpace.Model.ShipperShi;

namespace VeloSpace.Model.PayloadShi;

[Table("PAYLOAD")]
public class Payload
{
    [Key]
    [Column("PAYLOAD_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long PayloadId { get; set; }
    
    [Required]
    [Column("HEIGHT", TypeName = "numeric(5)")]
    public int Height { get; set; }
    
    [Required]
    [Column("WIDTH", TypeName = "numeric(5)")]
    public int Width { get; set; }
    
    [Required]
    [Column("DEPTH", TypeName = "numeric(5)")]
    public int Depth { get; set; }
    
    [Required]
    [Column("WEIGHT", TypeName = "numeric(5)")]
    public int Weight { get; set; }
    
    [Column("TRACKING_CODE")]
    [StringLength(55)]
    public string TrackingCode { get; set; }
    
    [Required]
    [Column("JUSTIFICATION")]
    [StringLength(500)]
    public string Justification { get; set; }
    
    [Required]
    [Column("PAYLOAD_STATUS_ID")]
    public long PayloadStatusId { get; set; }
    
    [ForeignKey(nameof(PayloadStatusId))]
    public PayloadStatus PayloadStatus { get; set; }
    
    [Required]
    [Column("SHIPPER_ID")]
    public long ShipperId { get; set; }
    
    [ForeignKey(nameof(ShipperId))]
    public Shipper Shipper { get; set; }
    
    [Required]
    [Column("ROCKET_ID")]
    public long RocketId { get; set; }
    
    [ForeignKey(nameof(RocketId))]
    public Rocket Rocket { get; set; }
    
    [Required]
    [Column("PAYLOAD_PRIORITY_ID")]
    public long PayloadPriorityId { get; set; }
    
    [ForeignKey(nameof(PayloadPriorityId))]
    public PayloadPriority PayloadPriority { get; set; }
    
    [Required]
    [Column("LAUNCH_PROVIDER_ID")]
    public long LaunchProviderId { get; set; }
    
    [ForeignKey(nameof(LaunchProviderId))]
    public LaunchProvider LaunchProvider { get; set; }
}
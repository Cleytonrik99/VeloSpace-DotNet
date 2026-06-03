using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VeloSpace.Model.OperatorShi;
using VeloSpace.Model.PayloadShi;
using VeloSpace.Model.SatelliteShi;

namespace VeloSpace.Model.InspectionShi;

[Table("INSPECTION")]
public class Inspection
{
    [Key]
    [Column("INSPECTION_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long InspectionId { get; set; }
    
    [Required]
    [Column("MEASURED_HEIGHT", TypeName = "numeric(5)")]
    public int MeasuredHeight { get; set; }
    
    [Required]
    [Column("MEASURED_WIDTH", TypeName = "numeric(5)")]
    public int MeasuredWidth { get; set; }
    
    [Required]
    [Column("MEASURED_LENGTH", TypeName = "numeric(5)")]
    public int MeasuredLength { get; set; }
    
    [Required]
    [Column("MEASURED_WEIGHT", TypeName = "numeric(5)")]
    public int MeasuredWeight { get; set; }
    
    [Required]
    [Column("INSPECTION_DATE", TypeName = "Date")]
    public DateTime InspectionDate { get; set; }
    
    [Required]
    [Column("RESULT")]
    [StringLength(1)]
    public string Result { get; set; }
    
    [Required]
    [Column("OPERATOR_ID")]
    public long OperatorId { get; set; }
    
    [ForeignKey(nameof(OperatorId))]
    public Operator Operator { get; set; }
    
    [Required]
    [Column("SATELLITE_ID")]
    public long SatelliteId { get; set; }
    
    [ForeignKey(nameof(SatelliteId))]
    public Satellite Satellite { get; set; }
}
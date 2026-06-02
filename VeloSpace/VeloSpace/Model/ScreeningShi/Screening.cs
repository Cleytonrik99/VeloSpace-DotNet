using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VeloSpace.Model.PayloadShi;

namespace VeloSpace.Model.ScreeningShi;

[Table("SCREENING")]
public class Screening
{
    [Key]
    [Column("SCREENING_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ScrreningId { get; set; }
    
    [Required]
    [Column("MEASURED_HEIGHT", TypeName = "numeric(5)")]
    public int MeasuredHeight { get; set; }
    
    [Required]
    [Column("MEASURED_WIDTH", TypeName = "numeric(5)")]
    public int MeasuredWidth { get; set; }
    
    [Required]
    [Column("MEASURED_DEPTH", TypeName = "numeric(5)")]
    public int MeasuredDepth { get; set; }
    
    [Required]
    [Column("MEASURED_WEIGHT", TypeName = "numeric(5)")]
    public int MeasuredWeight { get; set; }
    
    [Required]
    [Column("INSPECTION_DATE", TypeName = "Date")]
    public DateTime InspectionDate { get; set; }
    
    [Required]
    [Column("PAYLOAD_HANDLER_ID")]
    public long PayloadHandlerId { get; set; }
    
    [ForeignKey(nameof(PayloadHandlerId))]
    public PayloadHandler PayloadHandler { get; set; }
    
    [Required]
    [Column("PAYLOAD_ID")]
    public long PayloadId { get; set; }
    
    [ForeignKey(nameof(PayloadId))]
    public Payload Payload { get; set; }
}
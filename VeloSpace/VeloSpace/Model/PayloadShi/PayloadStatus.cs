using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeloSpace.Model.PayloadShi;

[Table("PAYLOAD_STATUS")]
public class PayloadStatus
{
    [Key]
    [Column("PAYLOAD_STATUS_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long PayloadStatusId { get; set; }
    
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(40)]
    public string Description { get; set; }
}
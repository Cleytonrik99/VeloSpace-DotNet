using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeloSpace.Model.PayloadShi;

[Table("HANDLER_STATUS")]
public class HandlerStatus
{
    [Key]
    [Column("HANDLER_STATUS_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long HandlerStatusId { get; set; }
    
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(30)]
    public string Description { get; set; }
}
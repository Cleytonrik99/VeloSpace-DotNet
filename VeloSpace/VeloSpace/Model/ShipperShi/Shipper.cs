using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace VeloSpace.Model.ShipperShi;

[Table("SHIPPER")]
[Index(nameof(DocumentShipper), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class Shipper
{
    [Key]
    [Column("SHIPPER_ID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ShipperId { get; set; }
    
    [Required]
    [Column("NAME")]
    [StringLength(50)]
    public string Name { get; set; }
    
    [Required]
    [Column("DOCUMENT_SHIPPER")]
    [StringLength(15)]
    public string DocumentShipper { get; set; }
    
    [Required]
    [Column("EMAIL")]
    [StringLength(255)]
    public string Email { get; set; }
    
    [Required]
    [Column("PHONE", TypeName = "numeric(15)")]
    public int Phone { get; set; }
    
    [Required]
    [Column("PASSWORD_HASH")]
    [StringLength(255)]
    public string PasswordHash { get; set; }
    
    [Required]
    [Column("TYPE")]
    [StringLength(2)]
    public string Type { get; set; }
}
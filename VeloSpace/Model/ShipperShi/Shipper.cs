using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using VeloSpace.Model.User;

namespace VeloSpace.Model.ShipperShi;

[Table("SHIPPER")]
[Index(nameof(ShipperDocument), IsUnique = true)]
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
    [Column("SHIPPER_DOCUMENT")]
    [StringLength(15)]
    public string ShipperDocument { get; set; }
    
    [Required]
    [Column("TYPE")]
    [StringLength(2)]
    public string Type { get; set; }
    
    [Required]
    [Column("USER_ACCOUNT_ID")]
    public long UserAccountId { get; set; }
    
    [ForeignKey(nameof(UserAccountId))]
    public UserAccount UserAccount { get; set; }
}
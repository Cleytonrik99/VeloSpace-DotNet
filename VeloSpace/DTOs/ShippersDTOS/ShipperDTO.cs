namespace VeloSpace.DTOs.Shippers;

public class ShipperDTO
{
    public long ShipperId { get; set; }
    public string Name { get; set; }
    public string ShipperDocument { get; set; }
    public string Type { get; set; }
    public long UserAccountId { get; set; }
}
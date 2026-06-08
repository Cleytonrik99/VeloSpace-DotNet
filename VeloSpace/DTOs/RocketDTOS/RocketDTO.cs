namespace VeloSpace.DTOs.RocketDTOS;

public class RocketDTO
{
    public long RocketId { get; set; }
    public string Name { get; set; }
    public int CapacityHeight { get; set; }
    public int CapacityWidth { get; set; }
    public int CapacityLength { get; set; }
    public int CapacityWeight { get; set; }
    public DateTime LaunchDate { get; set; }
    public long RocketStatusId { get; set; }
}
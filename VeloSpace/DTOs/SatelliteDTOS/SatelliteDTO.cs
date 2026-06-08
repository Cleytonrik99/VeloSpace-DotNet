namespace VeloSpace.DTOs.SatelliteDTOS;

public class SatelliteDTO
{
    public long SatelliteId { get; set; }
    public string Name { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public int Length { get; set; }
    public int Weight { get; set; }
    public string TrackingCode { get; set; }
    public string LaunchJustification { get; set; }
    public long SatellitePriorityId { get; set; }
    public long RocketId { get; set; }
    public long SatelliteStatusId { get; set; }
    public long ShipperId { get; set; }
    public long LaunchProviderId { get; set; }
}
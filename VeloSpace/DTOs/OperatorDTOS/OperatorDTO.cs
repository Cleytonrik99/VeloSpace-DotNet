namespace VeloSpace.DTOs.OperatorDTOS;

public class OperatorDTO
{
    public long OperatorId { get; set; }
    public string Name { get; set; }
    public string Cpf { get; set; }
    public long OperatorStatusId { get; set; }
    public long LaunchProviderId { get; set; }
    public long UserAccountId { get; set; }
}
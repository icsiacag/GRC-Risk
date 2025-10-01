using GRC.Application.Common.Models;

namespace GRC.Application.UseCases.Risks.Commands.CloseRisk;

public class CloseRiskCommand : ICommand
{
    public Guid Id { get; set; }
    public string ClosedBy { get; set; }
    public string Reason { get; set; }
}

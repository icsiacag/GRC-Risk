using GRC.Application.Common.Models;
using GRC.Domain.Enums;

namespace GRC.Application.UseCases.Risks.Commands.AssignControl;

public class AssignControlCommand : ICommand
{
    public Guid RiskId { get; set; }
    public string ControlName { get; set; }
    public string ControlDescription { get; set; }
    public ControlType ControlType { get; set; }
    public ControlEffectiveness Effectiveness { get; set; }
    public string ImplementedBy { get; set; }
    public bool IsAutomated { get; set; }
}

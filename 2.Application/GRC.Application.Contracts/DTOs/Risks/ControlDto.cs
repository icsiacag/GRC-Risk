using GRC.Domain.Enums;

namespace GRC.Application.Contracts.DTOs.Risks;

public class ControlDto
{
    public Guid ControlId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ControlType Type { get; set; }
    public ControlEffectiveness Effectiveness { get; set; }
    public ControlStatus Status { get; set; }
    public string ImplementedBy { get; set; } = string.Empty;
    public DateTime? ImplementationDate { get; set; }
    public DateTime? LastTestDate { get; set; }
    public DateTime? NextTestDate { get; set; }
    public int TestFrequencyDays { get; set; }
    public decimal? AnnualCost { get; set; }
    public bool IsAutomated { get; set; }
}

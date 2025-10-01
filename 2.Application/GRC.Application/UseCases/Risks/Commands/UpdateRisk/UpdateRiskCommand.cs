using GRC.Application.Common.Models;

namespace GRC.Application.UseCases.Risks.Commands.UpdateRisk;

public class UpdateRiskCommand : ICommand
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? Department { get; set; }
    public string? Process { get; set; }
    public string? Notes { get; set; }
}

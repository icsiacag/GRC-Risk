using GRC.Application.Common.Models;
using GRC.Domain.Entities.RiskManagement;
using GRC.Domain.Enums;

namespace GRC.Application.UseCases.Risks.Commands.CreateRisk;

public class CreateRiskCommand : ICommand<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid RiskCategoryId { get; set; }
    public RiskType Type { get; set; }
    public string OwnerId { get; set; }
    public int LikelihoodLevel { get; set; }
    public int ImpactLevel { get; set; }
    public string? Department { get; set; }
    public string? Process { get; set; }
}

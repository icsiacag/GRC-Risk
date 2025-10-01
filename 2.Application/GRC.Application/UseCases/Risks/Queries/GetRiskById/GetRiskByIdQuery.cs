using MediatR;
using GRC.Application.Common.Models;

namespace GRC.Application.UseCases.Risks.Queries.GetRiskById;

public class GetRiskByIdQuery : IRequest<RiskDto>
{
    public Guid Id { get; set; }
}

public class RiskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid RiskCategoryId { get; set; }
    public string RiskCategoryName { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string OwnerId { get; set; }
    public int InherentScore { get; set; }
    public int? ResidualScore { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastAssessmentDate { get; set; }
}

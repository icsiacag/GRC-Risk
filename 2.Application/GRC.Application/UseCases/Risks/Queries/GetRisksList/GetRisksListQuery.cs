using MediatR;
using GRC.Application.Common.Models;

namespace GRC.Application.UseCases.Risks.Queries.GetRisksList;

public class GetRisksListQuery : IRequest<PaginatedList<RiskListItemDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public Guid? RiskCategoryId { get; set; }
    public string? Status { get; set; }
    public string? OwnerId { get; set; }
}

public class RiskListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string RiskCategoryName { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public string OwnerId { get; set; }
    public int InherentScore { get; set; }
    public int? ResidualScore { get; set; }
    public DateTime CreatedAt { get; set; }
}

using MediatR;
using GRC.Domain.Interfaces.Repositories;
using GRC.Domain.Specifications;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace GRC.Application.UseCases.Risks.Queries.GetRisksList;

public class GetRisksListQueryHandler : IRequestHandler<GetRisksListQuery, PaginatedList<RiskListItemDto>>
{
    private readonly IRiskRepository _riskRepository;
    private readonly IMapper _mapper;

    public GetRisksListQueryHandler(IRiskRepository riskRepository, IMapper mapper)
    {
        _riskRepository = riskRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<RiskListItemDto>> Handle(GetRisksListQuery request, CancellationToken cancellationToken)
    {
        var (risks, totalCount) = await _riskRepository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            null, // predicate will be handled in repository
            x => x.CreatedAt, // order by creation date
            false, // descending
            cancellationToken);

        var riskDtos = _mapper.Map<List<RiskListItemDto>>(risks);
        return new PaginatedList<RiskListItemDto>(riskDtos, totalCount, request.PageNumber, request.PageSize);
    }
}

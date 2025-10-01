using MediatR;
using GRC.Domain.Interfaces.Repositories;
using AutoMapper;

namespace GRC.Application.UseCases.Risks.Queries.GetRiskById;

public class GetRiskByIdQueryHandler : IRequestHandler<GetRiskByIdQuery, RiskDto>
{
    private readonly IRiskRepository _riskRepository;
    private readonly IMapper _mapper;

    public GetRiskByIdQueryHandler(IRiskRepository riskRepository, IMapper mapper)
    {
        _riskRepository = riskRepository;
        _mapper = mapper;
    }

    public async Task<RiskDto> Handle(GetRiskByIdQuery request, CancellationToken cancellationToken)
    {
        var risk = await _riskRepository.GetByIdAsync(request.Id, cancellationToken);
        if (risk == null)
        {
            throw new Exception(""Risk not found."");
        }

        return _mapper.Map<RiskDto>(risk);
    }
}

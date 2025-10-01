using MediatR;
using GRC.Domain.Interfaces.Repositories;
using GRC.Domain.Entities.RiskManagement;

namespace GRC.Application.UseCases.Risks.Commands.CreateRisk;

public class CreateRiskCommandHandler : IRequestHandler<CreateRiskCommand, Guid>
{
    private readonly IRiskRepository _riskRepository;

    public CreateRiskCommandHandler(IRiskRepository riskRepository)
    {
        _riskRepository = riskRepository;
    }

    public async Task<Guid> Handle(CreateRiskCommand request, CancellationToken cancellationToken)
    {
        var risk = Risk.Create(
            request.Title,
            request.Description,
            request.RiskCategoryId,
            request.Type,
            request.OwnerId,
            request.LikelihoodLevel,
            request.ImpactLevel,
            request.Department,
            request.Process);

        await _riskRepository.AddAsync(risk, cancellationToken);
        await _riskRepository.SaveChangesAsync(cancellationToken);

        return risk.Id;
    }
}

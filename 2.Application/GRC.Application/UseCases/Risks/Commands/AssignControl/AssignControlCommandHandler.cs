using MediatR;
using GRC.Domain.Interfaces.Repositories;

namespace GRC.Application.UseCases.Risks.Commands.AssignControl;

public class AssignControlCommandHandler : IRequestHandler<AssignControlCommand>
{
    private readonly IRiskRepository _riskRepository;

    public AssignControlCommandHandler(IRiskRepository riskRepository)
    {
        _riskRepository = riskRepository;
    }

    public async Task Handle(AssignControlCommand request, CancellationToken cancellationToken)
    {
        var risk = await _riskRepository.GetByIdAsync(request.RiskId, cancellationToken);
        if (risk == null)
        {
            throw new Exception(""Risk not found."");
        }

        risk.AssignControl(
            request.ControlName,
            request.ControlDescription,
            request.ControlType,
            request.Effectiveness,
            request.ImplementedBy,
            request.IsAutomated);

        await _riskRepository.UpdateAsync(risk, cancellationToken);
        await _riskRepository.SaveChangesAsync(cancellationToken);
    }
}

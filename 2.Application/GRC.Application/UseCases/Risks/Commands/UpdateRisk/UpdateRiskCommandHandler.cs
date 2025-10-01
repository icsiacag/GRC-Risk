using MediatR;
using GRC.Domain.Interfaces.Repositories;

namespace GRC.Application.UseCases.Risks.Commands.UpdateRisk;

public class UpdateRiskCommandHandler : IRequestHandler<UpdateRiskCommand>
{
    private readonly IRiskRepository _riskRepository;

    public UpdateRiskCommandHandler(IRiskRepository riskRepository)
    {
        _riskRepository = riskRepository;
    }

    public async Task Handle(UpdateRiskCommand request, CancellationToken cancellationToken)
    {
        var risk = await _riskRepository.GetByIdAsync(request.Id, cancellationToken);
        if (risk == null)
        {
            throw new Exception(""Risk not found."");
        }

        risk.Update(
            request.Title,
            request.Description,
            risk.Type, // Type is not updated in this command
            request.Department,
            request.Process,
            request.Notes);

        await _riskRepository.UpdateAsync(risk, cancellationToken);
        await _riskRepository.SaveChangesAsync(cancellationToken);
    }
}

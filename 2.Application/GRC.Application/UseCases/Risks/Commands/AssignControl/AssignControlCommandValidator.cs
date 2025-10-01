using FluentValidation;
using GRC.Application.Common.Validators;

namespace GRC.Application.UseCases.Risks.Commands.AssignControl;

public class AssignControlCommandValidator : AbstractValidator<AssignControlCommand>
{
    public AssignControlCommandValidator()
    {
        RuleFor(x => x.ControlName)
            .NotEmpty().WithMessage(""Control name is required."")
            .MaximumLength(200).WithMessage(""Control name must not exceed 200 characters."");

        RuleFor(x => x.ControlDescription)
            .NotEmpty().WithMessage(""Control description is required."")
            .MaximumLength(1000).WithMessage(""Control description must not exceed 1000 characters."");

        RuleFor(x => x.ImplementedBy)
            .NotEmpty().WithMessage(""Implemented by is required."");

        RuleFor(x => x.RiskId)
            .NotEmpty().WithMessage(""Risk ID is required."");
    }
}

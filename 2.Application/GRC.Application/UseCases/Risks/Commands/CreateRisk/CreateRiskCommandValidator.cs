using FluentValidation;
using GRC.Application.Common.Validators;

namespace GRC.Application.UseCases.Risks.Commands.CreateRisk;

public class CreateRiskCommandValidator : AbstractValidator<CreateRiskCommand>
{
    public CreateRiskCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(""Title is required."")
            .MaximumLength(200).WithMessage(""Title must not exceed 200 characters."");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(""Description is required."")
            .MaximumLength(1000).WithMessage(""Description must not exceed 1000 characters."");

        RuleFor(x => x.RiskCategoryId)
            .NotEmpty().WithMessage(""Risk Category is required."");

        RuleFor(x => x.OwnerId)
            .NotEmpty().WithMessage(""Owner is required."");

        RuleFor(x => x.LikelihoodLevel)
            .InclusiveBetween(1, 5).WithMessage(""Likelihood must be between 1 and 5."");

        RuleFor(x => x.ImpactLevel)
            .InclusiveBetween(1, 5).WithMessage(""Impact must be between 1 and 5."");
    }
}

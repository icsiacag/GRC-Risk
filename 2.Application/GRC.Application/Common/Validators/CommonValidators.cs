using FluentValidation;

namespace GRC.Application.Common.Validators;

public static class CommonValidators
{
    public static IRuleBuilderOptions<T, string> NotEmptyWithMessage<T>(this IRuleBuilder<T, string> ruleBuilder, string propertyName)
    {
        return ruleBuilder.NotEmpty().WithMessage($\"{propertyName} is required.\");
    }

    public static IRuleBuilderOptions<T, string> MaximumLengthWithMessage<T>(this IRuleBuilder<T, string> ruleBuilder, int maxLength, string propertyName)
    {
        return ruleBuilder.MaximumLength(maxLength).WithMessage($\"{propertyName} must not exceed {maxLength} characters.\");
    }

    public static IRuleBuilderOptions<T, int> InclusiveBetweenWithMessage<T>(this IRuleBuilder<T, int> ruleBuilder, int min, int max, string propertyName)
    {
        return ruleBuilder.InclusiveBetween(min, max).WithMessage($\"{propertyName} must be between {min} and {max}.\");
    }
}

namespace GRC.Application.Common.Exceptions;

/// <summary>
/// Base application exception.
/// </summary>
public abstract class ApplicationException : Exception
{
    protected ApplicationException(string message) : base(message)
    {
    }

    protected ApplicationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Validation exception (FluentValidation hatalarını wrap eder).
/// </summary>
public class ValidationException : ApplicationException
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<FluentValidation.Results.ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }
}

/// <summary>
/// Not found exception.
/// </summary>
public class NotFoundException : ApplicationException
{
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}

/// <summary>
/// Forbidden access exception.
/// </summary>
public class ForbiddenAccessException : ApplicationException
{
    public ForbiddenAccessException()
        : base("Forbidden access.")
    {
    }

    public ForbiddenAccessException(string message)
        : base(message)
    {
    }
}

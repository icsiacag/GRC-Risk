namespace GRC.Domain.Exceptions;

public abstract class DomainException : Exception
{
    public string ErrorCode { get; protected set; } = string.Empty;
    public Dictionary<string, object>? Details { get; protected set; }

    protected DomainException(string message) : base(message) { }
    protected DomainException(string message, Exception innerException) : base(message, innerException) { }
    
    protected DomainException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
    
    protected DomainException(string message, string errorCode, Dictionary<string, object> details) : base(message)
    {
        ErrorCode = errorCode;
        Details = details;
    }
}

namespace GRC.Domain.Exceptions;

public class ComplianceFrameworkNotFoundException : DomainException
{
    public Guid FrameworkId { get; }

    public ComplianceFrameworkNotFoundException(Guid frameworkId)
        : base($\"Compliance framework with ID '{frameworkId}' was not found.\", \"FRAMEWORK_NOT_FOUND\")
    {
        FrameworkId = frameworkId;
    }
}

public class InsufficientEvidenceException : DomainException
{
    public Guid RequirementId { get; }

    public InsufficientEvidenceException(Guid requirementId)
        : base($\"Insufficient evidence for compliance requirement {requirementId}.\", \"INSUFFICIENT_EVIDENCE\")
    {
        RequirementId = requirementId;
        Details = new Dictionary<string, object> { { \"RequirementId\", requirementId } };
    }
}

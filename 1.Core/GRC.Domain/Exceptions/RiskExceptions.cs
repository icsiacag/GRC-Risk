namespace GRC.Domain.Exceptions;

public class RiskNotFoundException : DomainException
{
    public Guid RiskId { get; }

    public RiskNotFoundException(Guid riskId)
        : base($\"Risk with ID '{riskId}' was not found.\", \"RISK_NOT_FOUND\")
    {
        RiskId = riskId;
        Details = new Dictionary<string, object> { { \"RiskId\", riskId } };
    }
}

public class InvalidRiskScoreException : DomainException
{
    public int ProvidedScore { get; }

    public InvalidRiskScoreException(int score)
        : base($\"Invalid risk score: {score}. Score must be between 1 and 100.\", \"INVALID_RISK_SCORE\")
    {
        ProvidedScore = score;
        Details = new Dictionary<string, object> { { \"ProvidedScore\", score } };
    }
}

public class ClosedRiskOperationException : DomainException
{
    public Guid RiskId { get; }
    public string Operation { get; }

    public ClosedRiskOperationException(Guid riskId, string operation)
        : base($\"Cannot perform operation '{operation}' on closed risk {riskId}.\", \"CLOSED_RISK_OPERATION\")
    {
        RiskId = riskId;
        Operation = operation;
        Details = new Dictionary<string, object> { { \"RiskId\", riskId }, { \"Operation\", operation } };
    }
}

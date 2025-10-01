namespace GRC.Domain.Exceptions;

public class AuditPlanNotFoundException : DomainException
{
    public Guid AuditPlanId { get; }

    public AuditPlanNotFoundException(Guid auditPlanId)
        : base($\"Audit plan with ID '{auditPlanId}' was not found.\", \"AUDIT_PLAN_NOT_FOUND\")
    {
        AuditPlanId = auditPlanId;
    }
}

public class AuditScheduleConflictException : DomainException
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public string ConflictingAuditorId { get; }

    public AuditScheduleConflictException(DateTime startDate, DateTime endDate, string auditorId)
        : base($\"Audit schedule conflict for auditor {auditorId} between {startDate:d} and {endDate:d}.\",
              \"AUDIT_SCHEDULE_CONFLICT\")
    {
        StartDate = startDate;
        EndDate = endDate;
        ConflictingAuditorId = auditorId;
        Details = new Dictionary<string, object> { 
            { \"StartDate\", startDate }, 
            { \"EndDate\", endDate }, 
            { \"AuditorId\", auditorId } 
        };
    }
}

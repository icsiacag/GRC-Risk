using GRC.Application.Contracts.DTOs.Reports;
using GRC.Application.Services.GRC.Application.Contracts.DTOs.Reports;
using GRC.Domain.Interfaces.Repositories;

namespace GRC.Application.Services;

/// <summary>
/// Rapor üretim servisi.
/// </summary>
public interface IReportGenerationService
{
    Task<RiskReportDto> GenerateRiskReportAsync(RiskReportRequestDto request, CancellationToken cancellationToken = default);
    Task<ComplianceReportDto> GenerateComplianceReportAsync(Guid frameworkId, CancellationToken cancellationToken = default);
    Task<ExecutiveDashboardDto> GenerateExecutiveDashboardAsync(CancellationToken cancellationToken = default);
}

public class ReportGenerationService : IReportGenerationService
{
    private readonly IRiskRepository _riskRepository;
    private readonly IComplianceRepository _complianceRepository;
    private readonly IAuditRepository _auditRepository;

    public ReportGenerationService(
        IRiskRepository riskRepository,
        IComplianceRepository complianceRepository,
        IAuditRepository auditRepository)
    {
        _riskRepository = riskRepository;
        _complianceRepository = complianceRepository;
        _auditRepository = auditRepository;
    }

    public async Task<RiskReportDto> GenerateRiskReportAsync(
        RiskReportRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var risks = await _riskRepository.GetAllAsync(cancellationToken);

        // Filtreleme
        if (request.CategoryId.HasValue)
            risks = risks.Where(r => r.RiskCategoryId == request.CategoryId.Value).ToList();

        if (!string.IsNullOrEmpty(request.Department))
            risks = risks.Where(r => r.Department == request.Department).ToList();

        // İstatistikler
        var report = new RiskReportDto
        {
            GeneratedAt = DateTime.UtcNow,
            TotalRisks = risks.Count,
            HighRisks = risks.Count(r => r.InherentRisk.Level == Domain.Enums.RiskLevel.High ||
                                        r.InherentRisk.Level == Domain.Enums.RiskLevel.Critical),
            AverageScore = risks.Any() ? (int)risks.Average(r => r.InherentRisk.Score) : 0,
            RisksByLevel = risks.GroupBy(r => r.InherentRisk.Level)
                               .ToDictionary(g => g.Key, g => g.Count()),
            RisksByType = risks.GroupBy(r => r.Type)
                              .ToDictionary(g => g.Key, g => g.Count()),
            RisksByDepartment = risks.Where(r => !string.IsNullOrEmpty(r.Department))
                                     .GroupBy(r => r.Department!)
                                     .ToDictionary(g => g.Key, g => g.Count())
        };

        return report;
    }

    public async Task<ComplianceReportDto> GenerateComplianceReportAsync(
        Guid frameworkId,
        CancellationToken cancellationToken = default)
    {
        var framework = await _complianceRepository.GetWithRequirements(frameworkId, cancellationToken);

        if (framework == null)
            throw new InvalidOperationException($"Compliance framework not found: {frameworkId}");

        var report = new ComplianceReportDto
        {
            GeneratedAt = DateTime.UtcNow,
            FrameworkName = framework.Name,
            CompliancePercentage = framework.CompliancePercentage,
            TotalRequirements = framework.Requirements.Count,
            CompliantRequirements = framework.Requirements.Count(r =>
                r.Status?.Level == Domain.Enums.ComplianceLevel.FullyCompliant),
            NonCompliantRequirements = framework.Requirements.Count(r =>
                r.Status?.Level == Domain.Enums.ComplianceLevel.NonCompliant)
        };

        return report;
    }

    public async Task<ExecutiveDashboardDto> GenerateExecutiveDashboardAsync(
        CancellationToken cancellationToken = default)
    {
        var risks = await _riskRepository.GetAllAsync(cancellationToken);
        var activeRisks = risks.Where(r => r.Status != Domain.Enums.RiskStatus.Closed).ToList();

        var dashboard = new ExecutiveDashboardDto
        {
            GeneratedAt = DateTime.UtcNow,
            TotalActiveRisks = activeRisks.Count,
            CriticalRisks = activeRisks.Count(r => r.InherentRisk.Level == Domain.Enums.RiskLevel.Critical),
            HighRisks = activeRisks.Count(r => r.InherentRisk.Level == Domain.Enums.RiskLevel.High),
            AverageRiskScore = activeRisks.Any() ? (int)activeRisks.Average(r => r.InherentRisk.Score) : 0,
            RisksOverAppetite = activeRisks.Count(r => r.RiskAppetite != null &&
                r.ResidualRisk != null &&
                r.ResidualRisk.Score > r.RiskAppetite.AppetiteScore)
        };

        return dashboard;
    }
}

// Report DTOs
namespace GRC.Application.Contracts.DTOs.Reports
{
    public class RiskReportDto
    {
        public DateTime GeneratedAt { get; set; }
        public int TotalRisks { get; set; }
        public int HighRisks { get; set; }
        public int AverageScore { get; set; }
        public Dictionary<Domain.Enums.RiskLevel, int> RisksByLevel { get; set; } = new();
        public Dictionary<Domain.Enums.RiskType, int> RisksByType { get; set; } = new();
        public Dictionary<string, int> RisksByDepartment { get; set; } = new();
    }

    public class RiskReportRequestDto
    {
        public Guid? CategoryId { get; set; }
        public string? Department { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class ComplianceReportDto
    {
        public DateTime GeneratedAt { get; set; }
        public string FrameworkName { get; set; } = string.Empty;
        public int CompliancePercentage { get; set; }
        public int TotalRequirements { get; set; }
        public int CompliantRequirements { get; set; }
        public int NonCompliantRequirements { get; set; }
    }

    public class ExecutiveDashboardDto
    {
        public DateTime GeneratedAt { get; set; }
        public int TotalActiveRisks { get; set; }
        public int CriticalRisks { get; set; }
        public int HighRisks { get; set; }
        public int AverageRiskScore { get; set; }
        public int RisksOverAppetite { get; set; }
    }
}

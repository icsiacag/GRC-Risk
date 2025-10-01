using GRC.Domain.Entities.RiskManagement;
using GRC.Domain.Enums;
using GRC.Domain.Interfaces.Services;
using GRC.Domain.ValueObjects;

namespace GRC.Application.Services;

public class RiskCalculationService : IRiskCalculationService
{
    public RiskScore CalculateRiskScore(int likelihood, int impact)
    {
        return RiskScore.Create(likelihood, impact);
    }

    public RiskScore CalculateResidualRisk(RiskScore inherentRisk, IEnumerable<Control> controls)
    {
        var activeControls = controls.Where(c => c.Status == ControlStatus.Active).ToList();
        
        if (!activeControls.Any())
            return inherentRisk;

        var totalReduction = Math.Min(activeControls.Sum(c => c.GetReductionFactor()), 0.80m);
        
        var newLikelihood = (int)Math.Ceiling(inherentRisk.Likelihood * (1 - totalReduction / 2));
        var newImpact = (int)Math.Ceiling(inherentRisk.Impact * (1 - totalReduction / 2));
        
        newLikelihood = Math.Max(1, Math.Min(5, newLikelihood));
        newImpact = Math.Max(1, Math.Min(5, newImpact));
        
        return RiskScore.Create(newLikelihood, newImpact);
    }

    public bool IsRiskAboveAppetite(Risk risk)
    {
        if (risk.RiskAppetite == null || risk.ResidualRisk == null)
            return false;
            
        return risk.RiskAppetite.IsExceeded(risk.ResidualRisk.Score);
    }

    public decimal CalculateControlROI(Control control, RiskScore inherentRisk, RiskScore residualRisk)
    {
        if (control.AnnualCost == null || control.AnnualCost == 0)
            return 0;

        var riskReduction = inherentRisk.Score - residualRisk.Score;
        var roi = (riskReduction / control.AnnualCost.Value) * 100;
        
        return roi;
    }

    public int CalculatePortfolioScore(IEnumerable<Risk> risks)
    {
        var riskList = risks.ToList();
        if (!riskList.Any())
            return 0;

        var totalScore = riskList.Sum(r => r.ResidualRisk?.Score ?? r.InherentRisk.Score);
        return totalScore / riskList.Count;
    }

    public RiskAppetite SuggestRiskAppetite(IEnumerable<Risk> historicalRisks, string industry)
    {
        var risks = historicalRisks.ToList();
        var avgScore = risks.Any() ? (int)(risks.Average(r => r.InherentRisk.Score) * 0.8) : 50;

        var adjustedScore = industry.ToLower() switch
        {
            \"finance\" => (int)(avgScore * 0.7),
            \"technology\" => (int)(avgScore * 1.2),
            \"healthcare\" => (int)(avgScore * 0.8),
            _ => avgScore
        };

        adjustedScore = Math.Max(1, Math.Min(100, adjustedScore));
        
        return RiskAppetite.Create(adjustedScore, \"System\", 
            $\"Auto-suggested based on {risks.Count} historical risks and industry: {industry}\");
    }

    public DateTime SuggestNextReviewDate(Risk risk)
    {
        var monthsToAdd = (risk.ResidualRisk?.Level ?? risk.InherentRisk.Level) switch
        {
            RiskLevel.Critical => 1,
            RiskLevel.High => 3,
            RiskLevel.Medium => 6,
            RiskLevel.Low => 12,
            _ => 6
        };

        return DateTime.UtcNow.AddMonths(monthsToAdd);
    }
}

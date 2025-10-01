using GRC.Domain.Entities.RiskManagement;
using GRC.Domain.ValueObjects;

namespace GRC.Domain.Interfaces.Services;

public interface IRiskCalculationService
{
    RiskScore CalculateRiskScore(int likelihood, int impact);
    RiskScore CalculateResidualRisk(RiskScore inherentRisk, IEnumerable<Control> controls);
    bool IsRiskAboveAppetite(Risk risk);
    decimal CalculateControlROI(Control control, RiskScore inherentRisk, RiskScore residualRisk);
    int CalculatePortfolioScore(IEnumerable<Risk> risks);
    RiskAppetite SuggestRiskAppetite(IEnumerable<Risk> historicalRisks, string industry);
    DateTime SuggestNextReviewDate(Risk risk);
}

using GRC.Domain.Entities.RiskManagement;
using GRC.Domain.Enums;
using System.Linq.Expressions;

namespace GRC.Domain.Specifications;

public class HighRiskSpecification : Specification<Risk>
{
    public override Expression<Func<Risk, bool>> ToExpression()
    {
        return risk => risk.InherentRisk.Level == RiskLevel.High || 
                      risk.InherentRisk.Level == RiskLevel.Critical;
    }
}

public class ActiveRiskSpecification : Specification<Risk>
{
    public override Expression<Func<Risk, bool>> ToExpression()
    {
        return risk => risk.Status != RiskStatus.Closed && !risk.IsDeleted;
    }
}

public class OverdueForReviewSpecification : Specification<Risk>
{
    public override Expression<Func<Risk, bool>> ToExpression()
    {
        var today = DateTime.UtcNow.Date;
        return risk => risk.NextReviewDate.HasValue &&
                      risk.NextReviewDate.Value.Date <= today &&
                      risk.Status != RiskStatus.Closed;
    }
}

public class ExceedsRiskAppetiteSpecification : Specification<Risk>
{
    public override Expression<Func<Risk, bool>> ToExpression()
    {
        return risk => risk.RiskAppetite != null &&
                      risk.ResidualRisk != null &&
                      risk.ResidualRisk.Score > risk.RiskAppetite.AppetiteScore;
    }
}

public class RiskByCategorySpecification : Specification<Risk>
{
    private readonly Guid _categoryId;

    public RiskByCategorySpecification(Guid categoryId)
    {
        _categoryId = categoryId;
    }

    public override Expression<Func<Risk, bool>> ToExpression()
    {
        return risk => risk.RiskCategoryId == _categoryId;
    }
}

public class RiskByOwnerSpecification : Specification<Risk>
{
    private readonly string _ownerId;

    public RiskByOwnerSpecification(string ownerId)
    {
        _ownerId = ownerId;
    }

    public override Expression<Func<Risk, bool>> ToExpression()
    {
        return risk => risk.OwnerId == _ownerId;
    }
}

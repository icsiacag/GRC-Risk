using AutoMapper;
using GRC.Domain.Entities.RiskManagement;
using GRC.Application.UseCases.Risks.Queries.GetRiskById;
using GRC.Application.UseCases.Risks.Queries.GetRisksList;

namespace GRC.Application.Mappings;

public class RiskMappingProfile : Profile
{
    public RiskMappingProfile()
    {
        CreateMap<Risk, RiskDto>()
            .ForMember(dest => dest.RiskCategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.InherentScore, opt => opt.MapFrom(src => src.InherentRisk.Score))
            .ForMember(dest => dest.ResidualScore, opt => opt.MapFrom(src => src.ResidualRisk != null ? src.ResidualRisk.Score : (int?)null));

        CreateMap<Risk, RiskListItemDto>()
            .ForMember(dest => dest.RiskCategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.InherentScore, opt => opt.MapFrom(src => src.InherentRisk.Score))
            .ForMember(dest => dest.ResidualScore, opt => opt.MapFrom(src => src.ResidualRisk != null ? src.ResidualRisk.Score : (int?)null));
    }
}

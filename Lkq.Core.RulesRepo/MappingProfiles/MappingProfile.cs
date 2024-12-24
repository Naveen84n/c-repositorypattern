using AutoMapper;
using Lkq.Domain.RulesRepo;
using Lkq.Models.RulesRepo;

namespace Lkq.Core.RulesRepo.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PartTypes, PartTypesModel>()
               .ForMember(Pt => Pt.PartTypeID, vc => vc.MapFrom(sr => sr.Part_Codes_ID))
               .ForMember(Pt => Pt.PartTypeDescription, vc => vc.MapFrom(sr => sr.PartCodeDescription))
               .ForMember(Pt => Pt.PartTypeCode, vc => vc.MapFrom(sr => sr.PartCode))
               .ForMember(Pt => Pt.PartType, vc => vc.MapFrom(sr => sr.HollanderPartNo));

            CreateMap<Attributes, AttributesModel>()
                .ForMember(Am => Am.AttributeID, At => At.MapFrom(x => x.ACES_Attributes_ID));

            CreateMap<AttributeValues, AttributeValuesModel>()
                .ForMember(Bt => Bt.AttributeValueID, vc => vc.MapFrom(sr => sr.AttributeValue_ID));

            CreateMap<RuleRequestModel, PartRules>()
                .ForMember(dest => dest.ACES_Attributes_ID, vc => vc.MapFrom(sr => sr.AttributesID))
                .ForMember(dest => dest.Vindecoder_Source_ID, vc => vc.MapFrom(sr => sr.DataSourceID));
                //.ForMember(dest => dest.CreatedUser, vc => vc.MapFrom(sr => sr.User));

            CreateMap<PartRules, PartRulesModel>()
                .ForMember(Pr => Pr.RulesID, vc => vc.MapFrom(sr => sr.Part_Rules_ID))
                .ForMember(Pr => Pr.AttributesID, vc => vc.MapFrom(sr => sr.ACES_Attributes_ID))
                .ForMember(Pr => Pr.AttributeName, vc => vc.MapFrom(sr => sr.Attributes.TableName))
                .ForMember(Pr => Pr.DataSourceID, vc => vc.MapFrom(sr => sr.Vindecoder_Source_ID))
                .ForMember(Pr => Pr.DataSourceName, vc => vc.MapFrom(sr => sr.DataSource.Name));
        }
    }
}

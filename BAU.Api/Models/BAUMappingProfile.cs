using AutoMapper;
using BAU.Api.DAL.Models;

namespace BAU.Api.Models
{
    public class BAUMappingProfile : Profile
    {
        public BAUMappingProfile()
        {
            CreateMap<EngineerShiftModel, EngineerShift>();
            CreateMap<EngineerShift, EngineerShiftModel>();
            CreateMap<EngineerModel, Engineer>();
            CreateMap<Engineer, EngineerModel>();
        }
    }
}

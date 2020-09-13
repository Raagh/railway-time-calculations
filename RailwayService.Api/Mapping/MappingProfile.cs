using AutoMapper;

namespace RailwayService.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // API to Domain
            CreateMap<Model.Journey, Core.Domain.Journey>();

            // Domain to API
            CreateMap<Core.Domain.Journey, Model.Journey>();
        }    
    }
}

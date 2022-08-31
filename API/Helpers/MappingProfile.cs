using AutoMapper;
using Core.Dto;
using Core.Models;

namespace API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // CreateMap<Compania, CompaniaDto>();
            // CreateMap<CompaniaDto, Compania>();

            CreateMap<Compania, CompaniaDto>().ReverseMap();
        }
    }
}
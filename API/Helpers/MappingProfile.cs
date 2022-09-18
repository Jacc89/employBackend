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
            CreateMap<Empleado, EmpleadoUpsertDto>().ReverseMap();

            CreateMap<Empleado, EmpleadoReadDto>()
                       .ForMember(e => e.CompaniaId, m => m.MapFrom(c => c.Compania.NombreCompania));



        }
    }
}
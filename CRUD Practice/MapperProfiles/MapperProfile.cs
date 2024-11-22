using AutoMapper;
using CRUD_Practice.Models;

namespace CRUD_Practice.MapperProfiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        { 
            CreateMap<Person, PersonDto>().ReverseMap();
        }
    }
}

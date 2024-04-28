using AutoMapper;
using HotlListing.Dtos;
using HotlListing.Models;

namespace HotlListing.Configurations
{
    public class MapperInitinializer : Profile
    {
        public MapperInitinializer()
        {
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();
        }
    }
}

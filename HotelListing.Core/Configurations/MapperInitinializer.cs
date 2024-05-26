using AutoMapper;
using HotelListing.Core.Dtos;
using HotelListing.Data;

namespace HotelListing.Core.Configurations
{
    public class MapperInitinializer : Profile
    {
        public MapperInitinializer()
        {
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();
            CreateMap<ApiUser, UserDTO>().ReverseMap();

        }
    }
}

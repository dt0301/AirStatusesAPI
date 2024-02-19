using AirStatusesDomain;
using AutoMapper;
using AirStatusesApp.App.Dto;

namespace AirStatusesApp.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Flight, FlightDto>();
            CreateMap<FlightDto, Flight>();
        }
    }
}

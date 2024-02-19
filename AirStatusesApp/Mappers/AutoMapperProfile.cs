using AirStatusesDomain;
using AutoMapper;
using AirStatusesApp.App.Dto;

namespace AirStatusesApp.Mappers
{
    /// <summary>
    /// Класс AutoMapperProfile, наследуемый от базового класса Profile.
    /// Используется для настройки маппинга между объектами Flight и FlightDto.
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        /// <summary>
        /// Конструктор класса AutoMapperProfile.
        /// В нем определяются правила маппинга между объектами Flight и FlightDto.
        /// </summary>
        public AutoMapperProfile()
        {
            // Создает маппинг из объекта Flight в объект FlightDto.
            CreateMap<Flight, FlightDto>();
            // Создает маппинг из объекта FlightDto в объект Flight.
            CreateMap<FlightDto, Flight>();
        }
    }
}


using AirStatusesDomain;
using AutoMapper;
using AirStatusesData.Services;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using AirStatusesApp.App.Dto;
using AirStatusesInfrastructure.RedisService;

/// <summary>
/// Пространство имен AirStatusesApp.App.Flights.Queries содержит классы, связанные с запросами к данным о полетах.
/// </summary>
namespace AirStatusesApp.App.Flights.Queries
{
    /// <summary>
    /// Статический класс FlightQueryExtensions предоставляет методы расширения для IServiceCollection.
    /// </summary>
    public static class FlightQueryExtensions
    {
        /// <summary>
        /// Добавляет сервис запросов к данным о полетах в коллекцию сервисов.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        public static void AddFlightQueryService(this IServiceCollection services)
        {
            services.AddScoped<IFligtsQuery, FlightQuery>();
        }
    }

    /// <summary>
    /// Класс FlightQuery реализует интерфейс IFligtsQuery и предоставляет методы для работы с данными о полетах.
    /// </summary>
    public class FlightQuery : IFligtsQuery
    {
        private readonly IDataFlightsService _dataService;
        private readonly IMapper _mapper;
        private readonly ICache _cache;

        // TODO: Get CACHE_TYPE from Configuration key value
        private const string CACHE_TYPE = "flight";

        /// <summary>
        /// Конструктор для класса FlightQuery.
        /// </summary>
        /// <param name="dataService">Сервис для работы с данными о полетах.</param>
        /// <param name="mapper">Объект IMapper для преобразования данных.</param>
        /// <param name="redisCache">Объект ICache для работы с кэшем.</param>
        public FlightQuery(IDataFlightsService dataService, IMapper mapper, ICache redisCache)
        {
            _dataService = dataService;
            _mapper = mapper;
            _cache = redisCache;
        }

        /// <summary>
        /// Получает список всех полетов.
        /// </summary>
        /// <returns>Возвращает список всех полетов.</returns>
        public async Task<List<Flight>> GetFlightsAsync()
        {
            // TODO: Get CACHE_TYPE from Configuration key value
            var flights = await _cache.GetFlightsByPatternAsync($"{CACHE_TYPE}:*");
            if (flights.Count == 0)
            {
                var flightsFromDB = await _dataService.GetFlightsAsync();
                foreach (var flight in flightsFromDB)
                {
                    _cache.Set($"{CACHE_TYPE}:{flight.Id}", JsonConvert.SerializeObject(flight));
                }
                return flightsFromDB.ToList();
            }

            return flights.OrderBy(f => f.Id).ToList();
        }

        /// <summary>
        /// Получает список всех полетов в формате DTO.
        /// </summary>
        /// <returns>Возвращает список всех полетов в формате DTO.</returns>
        public async Task<List<FlightDto>> GetFlightsDtoAsync()
        {
            var flights = await GetFlightsAsync();
            var flightDtos = flights.Select(f => _mapper.Map<FlightDto>(f)).ToList();
            return flightDtos;
        }

        /// <summary>
        /// Получает полет по его идентификатору.
        /// </summary>
        /// <param name="Id">Идентификатор полета.</param>
        /// <returns>Возвращает полет или null, если полет не найден.</returns>
        public async Task<Flight?> GetFlightByIdAsync(int Id)
        {
            var flight = JsonConvert.DeserializeObject<Flight>(_cache.Get($"{CACHE_TYPE}:{Id}"));
            if (flight == null)
            {
                return await _dataService.GetFlightByIdFromDBAsync(Id);
            }

            return flight;
        }
    }
}

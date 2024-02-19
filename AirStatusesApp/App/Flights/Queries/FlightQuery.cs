using AirStatusesDomain;
using AirStatusesInfrastructure.Interfaces;
using AutoMapper;
using AirStatusesData.Services;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using AirStatusesApp.App.Dto;

namespace AirStatusesApp.App.Flights.Queries
{
    public static class FlightQueryExtensions
    {
        public static void AddFlightQueryService(this IServiceCollection services)
        {
            services.AddScoped<IFligtsQuery, FlightQuery>();
        }
    }

    public class FlightQuery : IFligtsQuery
    {
        private readonly IDataFlightsService _dataService;
        private readonly IMapper _mapper;
        private readonly ICache _cache;

        // TODO: Get CACHE_TYPE from Configuration key value
        private const string CACHE_TYPE = "flight";

        public FlightQuery(IDataFlightsService dataService, IMapper mapper, ICache redisCache)
        {
            _dataService = dataService;
            _mapper = mapper;
            _cache = redisCache;
        }

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

        public async Task<List<FlightDto>> GetFlightsDtoAsync()
        {
            var flights = await GetFlightsAsync();
            var flightDtos = flights.Select(f => _mapper.Map<FlightDto>(f)).ToList();
            return flightDtos;
        }

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

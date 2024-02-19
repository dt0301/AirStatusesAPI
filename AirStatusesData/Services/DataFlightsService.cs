using AirStatusesDomain;
using AirStatusesInfrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace AirStatusesData.Services
{
    //public static class DataFlightsExtensions
    //{
    //    public static void AddFlightData(this IServiceCollection services)
    //    {
    //        services.AddScoped<IDataFlightsService, DataFlightsService>();
    //    }
    //}

    public class DataFlightsService : IDataFlightsService
    {
        private readonly AppDbContext _context;
        private readonly ICache _cache;
        const string CACHE_TYPE = "flight";
        public DataFlightsService(AppDbContext context, ICache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<Flight>> GetFlightsAsync()
        {
            return await _context.Flights.ToListAsync();
        }

        public async Task<Flight?> GetFlightByIdFromDBAsync(int Id)
        {
            var flightFromBD = await _context.Flights.FirstOrDefaultAsync(f => f.Id == Id);
            if (flightFromBD != null)
            {
                _cache.Set($"{CACHE_TYPE}:{flightFromBD.Id}", JsonConvert.SerializeObject(flightFromBD));
                return flightFromBD;
            }

            return null;
        }

        public async Task<int> AddFlightAsync(Flight flight)
        {
            var lastKeyFligft = _context.Flights.Max(f => f.Id);
            flight.Id = ++lastKeyFligft;
            var item = await _context.Flights.AddAsync(flight);

            try
            {
                _context.SaveChanges();
                _cache.Set($"{CACHE_TYPE}:{flight.Id}", JsonConvert.SerializeObject(flight));
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сохранения в БД нового рейса: {ex.Message}", ex.InnerException);
            }

            return flight.Id;
        }

        /// <summary>
        /// Update Flight Status
        /// </summary>
        /// <param name="flightId"></param>
        /// <param name="flightStatus"></param>
        /// <returns></returns>
        public async Task<Flight?> UpdateFlightAsync(int flightId, FlightStatus flightStatus)
        {
            var flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == flightId);
            if (flight != null)
            {
                flight.Status = flightStatus;
                await _context.SaveChangesAsync();
                
                // Update RedisCahe
                _cache.Update($"{CACHE_TYPE}:{flight.Id}", JsonConvert.SerializeObject(flight));
                return flight;
            }

            return flight;
        }
    }
}

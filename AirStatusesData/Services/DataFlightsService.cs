using AirStatusesDomain;
using AirStatusesInfrastructure.RedisService;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AirStatusesData.Services
{
    /// <summary>
    /// Класс DataFlightsService представляет собой службу для работы с данными о рейсах.
    /// </summary>
    public class DataFlightsService : IDataFlightsService
    {
        private readonly AppDbContext _context;
        private readonly ICache _cache;
        const string CACHE_TYPE = "flight";

        /// <summary>
        /// Конструктор класса DataFlightsService.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        /// <param name="cache">Сервис кэширования.</param>
        public DataFlightsService(AppDbContext context, ICache cache)
        {
            _context = context;
            _cache = cache;
        }

        /// <summary>
        /// Получает список всех рейсов.
        /// </summary>
        /// <returns>Возвращает список всех рейсов.</returns>
        public async Task<IEnumerable<Flight>> GetFlightsAsync()
        {
            return await _context.Flights.ToListAsync();
        }

        /// <summary>
        /// Получает рейс по его идентификатору.
        /// </summary>
        /// <param name="Id">Идентификатор рейса.</param>
        /// <returns>Возвращает рейс или null, если рейс не найден.</returns>
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

        /// <summary>
        /// Добавляет новый рейс, и обновляет информацией о нем кеш.
        /// </summary>
        /// <param name="flight">Объект рейса для добавления.</param>
        /// <returns>Возвращает идентификатор добавленного рейса.</returns>
        public async Task<int> AddFlightAsync(Flight flight)
        {
            //TODO: Настроить EF to auto-increment
            var lastKeyFligft = _context.Flights.Max(f => f.Id);
            flight.Id = ++lastKeyFligft;
            var item = await _context.Flights.AddAsync(flight);

            try
            {
                _context.SaveChanges();
                _cache.Set($"{CACHE_TYPE}:{item.Entity.Id}", JsonConvert.SerializeObject(flight));
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сохранения в БД нового рейса: {ex.Message}", ex.InnerException);
            }

            return item.Entity.Id;
        }

        /// <summary>
        /// Обновляет статус рейса, и обновляет информацией о нем кеш.
        /// </summary>
        /// <param name="flightId">Идентификатор рейса.</param>
        /// <param name="flightStatus">Новый статус рейса.</param>
        /// <returns>Возвращает обновленный рейс или null, если рейс не найден.</returns>
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

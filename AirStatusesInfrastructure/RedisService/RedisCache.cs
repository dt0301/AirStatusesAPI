using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;
using AirStatusesDomain;

namespace AirStatusesInfrastructure.RedisService
{
    /// <summary>
    /// Расширение для IServiceCollection для добавления RedisCache в качестве ICache.
    /// </summary>
    public static class RedisCacheExtensions
    {
        public static void AddRedisCache(this IServiceCollection services)
        {
            services.AddScoped<ICache, RedisCache>();
        }
    }

    /// <summary>
    /// Реализация ICache, использующая Redis в качестве хранилища кэша.
    /// </summary>
    public class RedisCache : ICache
    {
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly IDatabase _db;

        public RedisCache(IConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
            _db = _redisConnection.GetDatabase();
        }

        /// <summary>
        /// Получает все рейсы, соответствующие заданному шаблону ключа.
        /// </summary>
        /// <param name="keyPattern">Шаблон ключа для поиска рейсов.</param>
        /// <returns>Список рейсов, соответствующих шаблону ключа.</returns>
        public async Task<List<Flight>> GetFlightsByPatternAsync(string keyPattern)
        {
            var server = _redisConnection.GetServer(_redisConnection.GetEndPoints().First());
            var keys = server.Keys(pattern: keyPattern);
            var flights = new List<Flight>();

            foreach (var key in keys)
            {
                string flightJson = Get(key); ;
                if (flightJson != null)
                {
                    var flight = JsonConvert.DeserializeObject<Flight>(flightJson);
                    flights.Add(flight);
                }
            }

            return flights;
        }

        /// <summary>
        /// Устанавливает значение в кэше по заданному ключу.
        /// </summary>
        /// <param name="key">Ключ для установки значения.</param>
        /// <param name="value">Значение для установки.</param>
        public void Set(string key, string value)
        {
            _db.StringSet(key, value);
        }

        /// <summary>
        /// Получает значение из кэша по заданному ключу.
        /// </summary>
        /// <param name="key">Ключ для получения значения.</param>
        /// <returns>Значение из кэша.</returns>
        public string Get(string key)
        {
            return _db.StringGet(key);
        }

        /// <summary>
        /// Удаляет значение из кэша по заданному ключу.
        /// </summary>
        /// <param name="key">Ключ для удаления значения.</param>
        public void Remove(string key)
        {
            _db.KeyDelete(key);
        }

        /// <summary>
        /// Обновляет значение в кэше по заданному ключу. Если ключ не существует, он будет создан.
        /// </summary>
        /// <param name="key">Ключ для обновления значения.</param>
        /// <param name="newValue">Новое значение для установки.</param>
        public void Update(string key, string newValue)
        {
            _db.StringSet(key, newValue);
        }
    }
}


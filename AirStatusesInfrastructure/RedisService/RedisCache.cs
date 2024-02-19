using AirStatusesInfrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;
using AirStatusesDomain;

namespace AirStatusesInfrastructure.RedisService
{
    public static class RedisCacheExtensions
    {
        public static void AddRedisCache(this IServiceCollection services)
        {
            services.AddScoped<ICache, RedisCache>();
        }
    }

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
        /// Получение всех рейсов по маске keyPattern, ("flight:*")
        /// </summary>
        /// <param name="keyPattern"></param>
        /// <returns></returns>
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

        public void Set(string key, string value)
        {
            _db.StringSet(key, value);
        }

        public string Get(string key)
        {
            return _db.StringGet(key);
        }

        public void Remove(string key)
        {
            _db.KeyDelete(key);
        }

        public void Update(string key, string newValue)
        {
            if (_db.KeyExists(key))
            {
                _db.StringSet(key, newValue);
            }
        }
    }
}

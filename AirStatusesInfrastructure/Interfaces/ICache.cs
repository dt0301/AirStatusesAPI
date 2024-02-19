using AirStatusesDomain;

namespace AirStatusesInfrastructure.Interfaces
{
    public interface ICache
    {
        void Set(string key, string value);
        void Update(string key, string newValue);
        string Get(string key);
        void Remove(string key);
        Task<List<Flight>> GetFlightsByPatternAsync(string keyPattern);
    }
}

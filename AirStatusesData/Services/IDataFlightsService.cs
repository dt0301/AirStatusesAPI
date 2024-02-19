using AirStatusesDomain;

namespace AirStatusesData.Services
{
    public interface IDataFlightsService
    {
        Task<IEnumerable<Flight>> GetFlightsAsync();
        Task<Flight?> GetFlightByIdFromDBAsync(int Id);
        Task<int> AddFlightAsync(Flight flight);
        Task<Flight?> UpdateFlightAsync(int flightId, FlightStatus flightStatus);
    }
}

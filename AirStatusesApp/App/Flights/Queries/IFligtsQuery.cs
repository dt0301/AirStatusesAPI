using AirStatusesApp.App.Dto;
using AirStatusesDomain;

namespace AirStatusesApp.App.Flights.Queries
{
    public interface IFligtsQuery
    {
        Task<List<Flight>> GetFlightsAsync();
        Task<List<FlightDto>> GetFlightsDtoAsync();
        Task<Flight?> GetFlightByIdAsync(int Id);
    }
}

using AirStatusesDomain;
using MediatR;

namespace AirStatusesApp.App.Flights.UpdateFlight
{
    public class UpdateFlightCommand : IRequest<Flight>
    {
        public int Id { get; set; }
        public FlightStatus Status { get; set; }
    }
}

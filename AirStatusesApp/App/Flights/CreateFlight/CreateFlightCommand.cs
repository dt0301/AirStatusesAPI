using AirStatusesApp.App.Dto;
using MediatR;

namespace AirStatusesApp.App.Flights.CreateFlight
{
    public class CreateFlightCommand : IRequest<int>
    {
        public FlightDto FlightDto { get; set; }
    }
}

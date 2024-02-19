using AirStatusesData.Services;
using MediatR;
using Newtonsoft.Json;
using AirStatusesInfrastructure.Interfaces;
using AirStatusesDomain;

namespace AirStatusesApp.App.Flights.CreateFlight
{
    public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, int>
    {
        private readonly IDataFlightsService _dataService;
        private readonly ICache _cache;
        private const string CACHE_TYPE = "flight";

        public CreateFlightCommandHandler(IDataFlightsService dataService, ICache cache)
        {
            _dataService = dataService;
            _cache = cache;
        }

        public async Task<int> Handle(CreateFlightCommand request, CancellationToken cancellationToken)
        {
            var flight = new Flight
            {
                Origin = request.FlightDto.Origin,
                Destination = request.FlightDto.Destination,
                Departure = request.FlightDto.Departure.ToUniversalTime(),
                Arrival = request.FlightDto.Arrival.ToUniversalTime(),
                Status = request.FlightDto.Status
            };

            var newFlightId = await _dataService.AddFlightAsync(flight);
            return newFlightId;
        }
    }
}

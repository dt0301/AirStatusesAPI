using AirStatusesDomain;
using AirStatusesInfrastructure.Interfaces;
using AirStatusesData.Services;
using MediatR;

namespace AirStatusesApp.App.Flights.UpdateFlight
{
    /// <summary>
    /// Обработчик команды на обновлние сатусов в СУБД и RedisCache
    /// </summary>
    public class UpdateFlightCommandHandler : IRequestHandler<UpdateFlightCommand, Flight>
    {
        private readonly IDataFlightsService _dataService;
        private readonly ICache _cache;
        private const string CACHE_TYPE = "flight";

        public UpdateFlightCommandHandler(IDataFlightsService dataService, ICache cache)
        {
            _dataService = dataService;
            _cache = cache;
        }

        /// <summary>
        /// Обработчик обновления статуса рейса.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Flight?> Handle(UpdateFlightCommand request, CancellationToken cancellationToken)
        {
            var flight = await _dataService.UpdateFlightAsync(request.Id, request.Status);

            if (flight != null)
            {
                return flight;
            }
            else
            {
                _cache.Remove($"{CACHE_TYPE}:{request.Id}");
                return null;
            }
        }
    }
}

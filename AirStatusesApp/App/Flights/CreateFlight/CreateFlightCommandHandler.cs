using AirStatusesData.Services;
using MediatR;
using AirStatusesDomain;
using AirStatusesInfrastructure.RedisService;

/// <summary>
/// Пространство имен AirStatusesApp.App.Flights.CreateFlight содержит классы, связанные с созданием полета.
/// </summary>
namespace AirStatusesApp.App.Flights.CreateFlight
{
    /// <summary>
    /// Класс CreateFlightCommandHandler обрабатывает команду создания полета.
    /// Этот класс реализует интерфейс IRequestHandler из библиотеки MediatR.
    /// </summary>
    public class CreateFlightCommandHandler : IRequestHandler<CreateFlightCommand, int>
    {
        private readonly IDataFlightsService _dataService;
        private readonly ICache _cache;
        private const string CACHE_TYPE = "flight";

        /// <summary>
        /// Конструктор для класса CreateFlightCommandHandler.
        /// </summary>
        /// <param name="dataService">Сервис для работы с данными о полетах.</param>
        /// <param name="cache">Сервис для работы с кэшем.</param>
        public CreateFlightCommandHandler(IDataFlightsService dataService, ICache cache)
        {
            _dataService = dataService;
            _cache = cache;
        }

        /// <summary>
        /// Метод Handle обрабатывает команду создания полета.
        /// </summary>
        /// <param name="request">Запрос на создание полета.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Возвращает идентификатор нового полета.</returns>
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

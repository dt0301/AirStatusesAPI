using AirStatusesDomain;
using AirStatusesData.Services;
using MediatR;
using AirStatusesInfrastructure.RedisService;

namespace AirStatusesApp.App.Flights.UpdateFlight
{
    /// <summary>
    /// Обработчик команды на обновление статусов в СУБД и RedisCache.
    /// </summary>
    public class UpdateFlightCommandHandler : IRequestHandler<UpdateFlightCommand, Flight>
    {
        // Сервис для работы с данными о рейсах.
        private readonly IDataFlightsService _dataService;
        // Интерфейс для работы с кэшем.
        private readonly ICache _cache;
        // Тип кэша.
        private const string CACHE_TYPE = "flight";

        /// <summary>
        /// Конструктор класса UpdateFlightCommandHandler.
        /// </summary>
        /// <param name="dataService">Сервис для работы с данными о рейсах.</param>
        /// <param name="cache">Интерфейс для работы с кэшем.</param>
        public UpdateFlightCommandHandler(IDataFlightsService dataService, ICache cache)
        {
            _dataService = dataService;
            _cache = cache;
        }

        /// <summary>
        /// Обработчик обновления статуса рейса.
        /// </summary>
        /// <param name="request">Команда обновления рейса.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Возвращает обновленный рейс, если обновление прошло успешно, иначе null, и удаляет запись из кеша</returns>
        public async Task<Flight?> Handle(UpdateFlightCommand request, CancellationToken cancellationToken)
        {
            // Обновление статуса рейса в базе данных.
            var flight = await _dataService.UpdateFlightAsync(request.Id, request.Status);

            // Если рейс успешно обновлен, возвращаем его, иначе удаляем из кэша и возвращаем null.
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


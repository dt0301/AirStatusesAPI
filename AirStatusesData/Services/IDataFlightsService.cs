using AirStatusesDomain;

namespace AirStatusesData.Services
{
    /// <summary>
    /// Интерфейс IDataFlightsService представляет собой контракт для службы работы с данными о рейсах.
    /// </summary>
    public interface IDataFlightsService
    {
        /// <summary>
        /// Получает список всех рейсов.
        /// </summary>
        /// <returns>Возвращает задачу, результатом которой является список всех рейсов.</returns>
        Task<IEnumerable<Flight>> GetFlightsAsync();

        /// <summary>
        /// Получает рейс по его идентификатору.
        /// </summary>
        /// <param name="Id">Идентификатор рейса.</param>
        /// <returns>Возвращает задачу, результатом которой является рейс или null, если рейс не найден.</returns>
        Task<Flight?> GetFlightByIdFromDBAsync(int Id);

        /// <summary>
        /// Добавляет новый рейс.
        /// </summary>
        /// <param name="flight">Объект рейса для добавления.</param>
        /// <returns>Возвращает задачу, результатом которой является идентификатор добавленного рейса.</returns>
        Task<int> AddFlightAsync(Flight flight);

        /// <summary>
        /// Обновляет статус рейса.
        /// </summary>
        /// <param name="flightId">Идентификатор рейса.</param>
        /// <param name="flightStatus">Новый статус рейса.</param>
        /// <returns>Возвращает задачу, результатом которой является обновленный рейс или null, если рейс не найден.</returns>
        Task<Flight?> UpdateFlightAsync(int flightId, FlightStatus flightStatus);
    }
}


using AirStatusesApp.App.Dto; /// <summary> Используется для доступа к классам DTO (Data Transfer Object) приложения AirStatusesApp. </summary>
using AirStatusesDomain; /// <summary> Используется для доступа к классам домена AirStatuses. </summary>

namespace AirStatusesApp.App.Flights.Queries
{
    /// <summary>
    /// Определение интерфейса IFligtsQuery, который содержит методы для выполнения запросов к данным о полетах.
    /// </summary>
    public interface IFligtsQuery
    {
        /// <summary>
        /// Метод для асинхронного получения списка всех полетов.
        /// Параметры: Нет.
        /// Возвращает: Список объектов Flight.
        /// </summary>
        Task<List<Flight>> GetFlightsAsync();

        /// <summary>
        /// Метод для асинхронного получения списка всех полетов в формате DTO.
        /// Параметры: Нет.
        /// Возвращает: Список объектов FlightDto.
        /// </summary>
        Task<List<FlightDto>> GetFlightsDtoAsync();

        /// <summary>
        /// Метод для асинхронного получения информации о полете по его идентификатору.
        /// Параметры: Id - идентификатор полета.
        /// Возвращает: Объект Flight или null, если полет с таким идентификатором не найден.
        /// </summary>
        Task<Flight?> GetFlightByIdAsync(int Id);
    }
}


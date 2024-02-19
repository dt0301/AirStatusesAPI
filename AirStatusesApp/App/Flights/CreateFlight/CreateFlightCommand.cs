using AirStatusesApp.App.Dto;
using MediatR;

/// <summary>
/// Пространство имен AirStatusesApp.App.Flights.CreateFlight содержит классы, связанные с созданием полета.
/// </summary>
namespace AirStatusesApp.App.Flights.CreateFlight
{
    /// <summary>
    /// Класс CreateFlightCommand представляет команду для создания полета.
    /// Этот класс реализует интерфейс IRequest из библиотеки MediatR, что позволяет его использовать в качестве запроса в шине команд/запросов MediatR.
    /// </summary>
    public class CreateFlightCommand : IRequest<int>
    {
        /// <summary>
        /// Объект FlightDto, содержащий данные о полете, который нужно создать.
        /// </summary>
        public FlightDto FlightDto { get; set; }
    }
}

using AirStatusesDomain;
using MediatR;

namespace AirStatusesApp.App.Flights.UpdateFlight
{
    /// <summary>
    /// Класс UpdateFlightCommand представляет команду обновления статуса полета.
    /// Этот класс реализует интерфейс IRequest с возвращаемым типом Flight.
    /// </summary>
    public class UpdateFlightCommand : IRequest<Flight>
    {
        /// <summary>
        /// Идентификатор полета, который нужно обновить.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Новый статус полета.
        /// </summary>
        public FlightStatus Status { get; set; }
    }

}

using AirStatusesDomain;

namespace AirStatusesApp.App.Dto
{
    /// <summary>
    /// Класс FlightDto представляет собой модель данных для информации о полете.
    /// </summary>
    public class FlightDto
    {
        /// <summary>
        /// Место отправления рейса.
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// Место назначения рейса.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Время отправления рейса.
        /// </summary>
        public DateTimeOffset Departure { get; set; }

        /// <summary>
        /// Время прибытия рейса.
        /// </summary>
        public DateTimeOffset Arrival { get; set; }

        /// <summary>
        /// Статус рейса.
        /// </summary>
        public FlightStatus Status { get; set; }
    }
}
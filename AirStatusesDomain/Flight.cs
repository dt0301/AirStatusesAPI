using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirStatusesDomain
{
    /// <summary>
    /// Класс Flight представляет модель данных рейса в базе данных.
    /// </summary>
    public class Flight
    {
        /// <summary>
        /// Уникальный идентификатор рейса. Это поле является ключом в базе данных.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Место отправления рейса. Максимальная длина этого поля составляет 256 символов.
        /// </summary>
        [MaxLength(256)]
        public string Origin { get; set; }

        /// <summary>
        /// Место назначения рейса. Максимальная длина этого поля составляет 256 символов.
        /// </summary>
        [MaxLength(256)]
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

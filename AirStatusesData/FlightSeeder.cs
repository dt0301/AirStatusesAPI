using AirStatusesDomain;
using Newtonsoft.Json;

namespace AirStatusesData
{
    /// <summary>
    /// Класс FlightSeeder используется для заполнения базы данных начальными данными о рейсах.
    /// </summary>
    public class FlightSeeder
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Конструктор класса FlightSeeder.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        public FlightSeeder(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Метод Seed используется для заполнения базы данных начальными данными о рейсах.
        /// </summary>
        public void Seed()
        {
            // Проверка, есть ли уже данные о рейсах в базе данных
            if (!_context.Flights.Any())
            {
                // Чтение данных о рейсах из JSON-файла
                var flightsData = ReadFlightsFromJson();

                // Преобразование даты и времени в UTC
                foreach (var flight in flightsData)
                {
                    flight.Departure = flight.Departure.ToUniversalTime();
                    flight.Arrival = flight.Arrival.ToUniversalTime();
                }

                // Добавление данных о рейсах в базу данных
                _context.Flights.AddRange(flightsData);
                // Сохранение изменений в базе данных
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Метод ReadFlightsFromJson используется для чтения данных о рейсах из JSON-файла.
        /// </summary>
        /// <returns>Список рейсов.</returns>
        private List<Flight>? ReadFlightsFromJson()
        {
            // Путь к JSON-файлу с данными о рейсах
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "flights.json");
            // Чтение JSON-файла
            var json = File.ReadAllText(path);
            // Десериализация JSON-файла в список рейсов
            var flights = JsonConvert.DeserializeObject<List<Flight>>(json);
            return flights;
        }
    }
}


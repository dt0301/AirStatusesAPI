using AirStatusesDomain;
using Newtonsoft.Json;

namespace AirStatusesData
{
    public class FlightSeeder
    {
        private readonly AppDbContext _context;

        public FlightSeeder(AppDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (!_context.Flights.Any())
            {
                var flightsData = ReadFlightsFromJson();

                // Преобразование даты и времени в UTC
                foreach (var flight in flightsData)
                {
                    flight.Departure = flight.Departure.ToUniversalTime();
                    flight.Arrival = flight.Arrival.ToUniversalTime();
                }

                _context.Flights.AddRange(flightsData);
                _context.SaveChanges();
            }
        }

        private List<Flight>? ReadFlightsFromJson()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "flights.json");
            var json = File.ReadAllText(path);
            var flights = JsonConvert.DeserializeObject<List<Flight>>(json);
            return flights;
        }
    }
}

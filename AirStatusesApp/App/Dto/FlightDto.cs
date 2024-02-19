using AirStatusesDomain;

namespace AirStatusesApp.App.Dto
{
    public class FlightDto
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTimeOffset Departure { get; set; }
        public DateTimeOffset Arrival { get; set; }
        public FlightStatus Status { get; set; }
    }
}